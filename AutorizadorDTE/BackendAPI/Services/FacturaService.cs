using System.Globalization;
using System.Text.RegularExpressions;
using AutorizadorDTE.BackendAPI.DTOs;
using AutorizadorDTE.BackendAPI.Models;

namespace AutorizadorDTE.BackendAPI.Services
{
    public class FacturaService
    {
        private const string ArchivoFacturas = "facturas.json";
        private readonly AlmacenamientoService almacenamiento;
        private readonly RTUService rtuService;
        private static readonly Regex PatronFecha = new(
            @"(?<!\d)(?<fecha>\d{1,2}/\d{1,2}/\d{4})\s+(?<hora>\d{1,2}:\d{2})(?!\d)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public FacturaService(AlmacenamientoService almacenamiento, RTUService rtuService)
        {
            this.almacenamiento = almacenamiento;
            this.rtuService = rtuService;
        }

        public async Task<RespuestaAutorizacionesDto> ProcesarAsync(
            List<SolicitudFacturaDto> solicitudes)
        {
            RespuestaAutorizacionesDto respuesta = new RespuestaAutorizacionesDto();
            ResultadoAutorizacionesDto resultado = respuesta.ResultadoAutorizaciones;
            List<RegistroFacturaDto> registros = await almacenamiento.LeerAsync(
                ArchivoFacturas,
                new List<RegistroFacturaDto>());
            HashSet<string> referencias = new HashSet<string>(
                registros.Select(registro => registro.Referencia),
                StringComparer.Ordinal);
            TimeZoneInfo zonaGuatemala = ObtenerZonaGuatemala();
            DateTime fechaAutorizacion = TimeZoneInfo.ConvertTime(
                DateTimeOffset.UtcNow,
                zonaGuatemala).Date;
            long correlativo = ObtenerUltimoCorrelativo(registros, fechaAutorizacion);

            foreach (SolicitudFacturaDto solicitud in solicitudes)
            {
                if (solicitud is null ||
                    !IntentarExtraerFecha(solicitud.TIEMPO, out DateTime fechaEmision))
                {
                    resultado.CantidadDescartadasPorFormato++;
                    continue;
                }

                HashSet<string> errores = new HashSet<string>(StringComparer.Ordinal);
                string referencia = (solicitud.REFERENCIA ?? string.Empty).Trim();
                string nitEmisor = NormalizarNit(solicitud.NIT_EMISOR);
                string nitReceptor = NormalizarNit(solicitud.NIT_RECEPTOR);

                if (string.IsNullOrWhiteSpace(referencia) || referencia.Length > 40)
                {
                    errores.Add("referenciaInvalida");
                    resultado.CantidadAutorizacionesRechazadasPorReferenciaInvalida++;
                }
                else if (referencias.Contains(referencia))
                {
                    errores.Add("referenciaDuplicada");
                    resultado.CantidadAutorizacionesRechazadasPorReferenciaDuplicada++;
                }

                if (!string.IsNullOrWhiteSpace(referencia))
                {
                    referencias.Add(referencia);
                }

                if (!ValidadorNIT.Validar(nitEmisor))
                {
                    errores.Add("nitEmisorInvalido");
                    resultado.CantidadAutorizacionesRechazadasPorNitEmisorInvalido++;
                }
                else if (!await rtuService.ExisteContribuyenteAsync(nitEmisor))
                {
                    errores.Add("nitEmisorInexistente");
                    resultado.CantidadAutorizacionesRechazadasPorNitEmisorInexistente++;
                }

                if (!ValidadorNIT.Validar(nitReceptor))
                {
                    errores.Add("nitReceptorInvalido");
                    resultado.CantidadAutorizacionesRechazadasPorNitReceptorInvalido++;
                }
                else if (!await rtuService.ExisteContribuyenteAsync(nitReceptor))
                {
                    errores.Add("nitReceptorInexistente");
                    resultado.CantidadAutorizacionesRechazadasPorNitReceptorInexistente++;
                }

                decimal ivaEsperado = Math.Round(
                    solicitud.VALOR * 0.12m,
                    2,
                    MidpointRounding.AwayFromZero);

                if (solicitud.VALOR <= 0 ||
                    decimal.Round(solicitud.VALOR, 2) != solicitud.VALOR ||
                    solicitud.IVA != ivaEsperado)
                {
                    if (solicitud.IVA != ivaEsperado)
                    {
                        errores.Add("ivaMalCalculado");
                        resultado.CantidadAutorizacionesRechazadasPorIvaMalCalculado++;
                    }

                    if (solicitud.VALOR <= 0 ||
                        decimal.Round(solicitud.VALOR, 2) != solicitud.VALOR)
                    {
                        errores.Add("valorInvalido");
                    }
                }

                if (solicitud.TOTAL != solicitud.VALOR + solicitud.IVA)
                {
                    errores.Add("totalMalCalculado");
                    resultado.CantidadAutorizacionesRechazadasPorTotalMalCalculado++;
                }

                Factura factura = new Factura(
                    referencia,
                    fechaEmision,
                    new Contribuyente(null, nitEmisor),
                    new Contribuyente(null, nitReceptor),
                    solicitud.VALOR,
                    solicitud.IVA,
                    solicitud.TOTAL);

                if (errores.Count == 0)
                {
                    correlativo++;
                    long codigo = long.Parse(
                        fechaAutorizacion.ToString("yyyyMMdd", CultureInfo.InvariantCulture) +
                        correlativo.ToString("D8", CultureInfo.InvariantCulture),
                        CultureInfo.InvariantCulture);
                    factura.setAutorizada(true);
                    factura.setCodigoAutorizacion(codigo);
                    resultado.CantidadAutorizacionesAprobadas++;
                    resultado.DetalleAprobaciones.Add(new DetalleAprobacionDto
                    {
                        Referencia = referencia,
                        CodigoAprobacion = codigo
                    });
                }
                else
                {
                    factura.setAutorizada(false);
                    resultado.CantidadAutorizacionesRechazadas++;
                }

                registros.Add(new RegistroFacturaDto
                {
                    TiempoOriginal = solicitud.TIEMPO ?? string.Empty,
                    FechaEmision = factura.getFechaEmision() ?? fechaEmision,
                    Referencia = factura.getNumeroFactura() ?? referencia,
                    NitEmisor = factura.getEmisor()?.getNIT() ?? nitEmisor,
                    NitReceptor = factura.getReceptor()?.getNIT() ?? nitReceptor,
                    Valor = factura.getValor() ?? solicitud.VALOR,
                    Iva = factura.getIVA() ?? solicitud.IVA,
                    Total = factura.getTotal() ?? solicitud.TOTAL,
                    Autorizada = factura.getAutorizada() == true,
                    CodigoAutorizacion = factura.getCodigoAutorizacion(),
                    Errores = errores.OrderBy(error => error, StringComparer.Ordinal).ToList()
                });
            }

            await almacenamiento.GuardarAsync(ArchivoFacturas, registros);
            return respuesta;
        }

        public async Task InicializarAsync()
        {
            await almacenamiento.GuardarAsync(
                ArchivoFacturas,
                new List<RegistroFacturaDto>());
        }

        private static string NormalizarNit(string? nit)
        {
            return (nit ?? string.Empty).Trim().ToUpperInvariant();
        }

        private static bool IntentarExtraerFecha(string? texto, out DateTime fecha)
        {
            fecha = default;

            if (string.IsNullOrWhiteSpace(texto))
            {
                return false;
            }

            Match coincidencia = PatronFecha.Match(texto);

            if (!coincidencia.Success)
            {
                return false;
            }

            string valor = coincidencia.Groups["fecha"].Value +
                " " + coincidencia.Groups["hora"].Value;
            string[] formatos =
            {
                "d/M/yyyy H:mm",
                "d/M/yyyy HH:mm",
                "dd/MM/yyyy H:mm",
                "dd/MM/yyyy HH:mm"
            };

            return DateTime.TryParseExact(
                valor,
                formatos,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fecha);
        }

        private static long ObtenerUltimoCorrelativo(
            List<RegistroFacturaDto> registros,
            DateTime fechaAutorizacion)
        {
            string prefijo = fechaAutorizacion.ToString(
                "yyyyMMdd",
                CultureInfo.InvariantCulture);
            long maximo = 0;

            foreach (RegistroFacturaDto registro in registros)
            {
                if (registro.CodigoAutorizacion is long codigo)
                {
                    string texto = codigo.ToString(CultureInfo.InvariantCulture);

                    if (texto.Length == 16 &&
                        texto.StartsWith(prefijo, StringComparison.Ordinal) &&
                        long.TryParse(texto.Substring(8), out long correlativo) &&
                        correlativo > maximo)
                    {
                        maximo = correlativo;
                    }
                }
            }

            return maximo;
        }

        private static TimeZoneInfo ObtenerZonaGuatemala()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("America/Guatemala");
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.FindSystemTimeZoneById(
                    "Central America Standard Time");
            }
        }
    }
}
