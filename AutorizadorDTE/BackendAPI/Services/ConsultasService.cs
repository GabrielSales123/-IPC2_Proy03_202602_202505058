using System.Globalization;
using AutorizadorDTE.BackendAPI.DTOs;

namespace AutorizadorDTE.BackendAPI.Services
{
    public class ConsultasService
    {
        private const string ArchivoFacturas = "facturas.json";
        private readonly AlmacenamientoService almacenamiento;

        public ConsultasService(AlmacenamientoService almacenamiento)
        {
            this.almacenamiento = almacenamiento;
        }

        public async Task<ConsultaContribuyenteDto> ConsultarContribuyenteAsync(
            string nit,
            string tipo,
            DateTime fechaInicio,
            DateTime fechaFin,
            CancellationToken cancellationToken)
        {
            List<RegistroFacturaDto> facturas = await LeerFacturasAsync(cancellationToken);
            bool emitidas = tipo.Equals("emitidas", StringComparison.OrdinalIgnoreCase);
            string nitNormalizado = nit.Trim();

            List<RegistroFacturaDto> facturasFiltradas = facturas
                .Where(factura => factura.Autorizada)
                .Where(factura => emitidas
                    ? factura.NitEmisor.Equals(nitNormalizado, StringComparison.OrdinalIgnoreCase)
                    : factura.NitReceptor.Equals(nitNormalizado, StringComparison.OrdinalIgnoreCase))
                .Where(factura => EstaEnRango(factura.FechaEmision, fechaInicio, fechaFin))
                .ToList();

            ConsultaContribuyenteDto respuesta = new ConsultaContribuyenteDto
            {
                Nit = nitNormalizado,
                Tipo = emitidas ? "emitidas" : "recibidas",
                FechaInicio = FormatearFecha(fechaInicio),
                FechaFin = FormatearFecha(fechaFin),
                TotalFacturas = facturasFiltradas.Count,
                MontoTotalRango = facturasFiltradas.Sum(factura => factura.Total),
                Detalle = facturasFiltradas
                    .GroupBy(factura => factura.FechaEmision.Date)
                    .OrderBy(grupo => grupo.Key)
                    .Select(grupo => new DetalleContribuyenteDto
                    {
                        Fecha = FormatearFecha(grupo.Key),
                        CantidadFacturas = grupo.Count(),
                        MontoTotal = grupo.Sum(factura => factura.Total)
                    })
                    .ToList()
            };

            return respuesta;
        }

        public async Task<ResumenIvaDto> ConsultarResumenIvaAsync(
            DateTime fecha,
            CancellationToken cancellationToken)
        {
            List<RegistroFacturaDto> facturas = await LeerFacturasAsync(cancellationToken);
            Dictionary<string, DetalleIvaDto> resumen = new(StringComparer.OrdinalIgnoreCase);

            foreach (RegistroFacturaDto factura in facturas.Where(factura =>
                factura.Autorizada && factura.FechaEmision.Date == fecha.Date))
            {
                AgregarIva(resumen, factura.NitEmisor, factura.Iva, true);
                AgregarIva(resumen, factura.NitReceptor, factura.Iva, false);
            }

            return new ResumenIvaDto
            {
                Fecha = FormatearFecha(fecha),
                Detalle = resumen.Values
                    .OrderBy(detalle => detalle.Nit, StringComparer.OrdinalIgnoreCase)
                    .ToList()
            };
        }

        public async Task<ResumenRangoDto> ConsultarResumenRangoAsync(
            DateTime fechaInicio,
            DateTime fechaFin,
            string modo,
            CancellationToken cancellationToken)
        {
            List<RegistroFacturaDto> facturas = await LeerFacturasAsync(cancellationToken);
            bool incluirIva = modo.Equals("total", StringComparison.OrdinalIgnoreCase);
            List<RegistroFacturaDto> facturasFiltradas = facturas
                .Where(factura => factura.Autorizada)
                .Where(factura => EstaEnRango(factura.FechaEmision, fechaInicio, fechaFin))
                .ToList();

            List<DetalleResumenRangoDto> detalle = facturasFiltradas
                .GroupBy(factura => factura.FechaEmision.Date)
                .OrderBy(grupo => grupo.Key)
                .Select(grupo => new DetalleResumenRangoDto
                {
                    Fecha = FormatearFecha(grupo.Key),
                    Monto = grupo.Sum(factura => incluirIva
                        ? factura.Total
                        : factura.Valor)
                })
                .ToList();

            return new ResumenRangoDto
            {
                FechaInicio = FormatearFecha(fechaInicio),
                FechaFin = FormatearFecha(fechaFin),
                Modo = incluirIva ? "total" : "sinIva",
                Detalle = detalle,
                TotalRango = detalle.Sum(dia => dia.Monto)
            };
        }

        public async Task<ReporteErroresDto> ConsultarReporteErroresAsync(
            DateTime fechaInicio,
            DateTime fechaFin,
            CancellationToken cancellationToken)
        {
            List<RegistroFacturaDto> facturas = await LeerFacturasAsync(cancellationToken);
            List<RegistroFacturaDto> facturasFiltradas = facturas
                .Where(factura => EstaEnRango(factura.FechaEmision, fechaInicio, fechaFin))
                .ToList();

            return new ReporteErroresDto
            {
                FechaInicio = FormatearFecha(fechaInicio),
                FechaFin = FormatearFecha(fechaFin),
                Detalle = facturasFiltradas
                    .GroupBy(factura => factura.FechaEmision.Date)
                    .OrderBy(grupo => grupo.Key)
                    .Select(grupo => new DetalleReporteErroresDto
                    {
                        Fecha = FormatearFecha(grupo.Key),
                        FacturasRecibidas = grupo.Count(),
                        NitEmisorInvalido = ContarError(grupo, "nitEmisorInvalido"),
                        NitReceptorInvalido = ContarError(grupo, "nitReceptorInvalido"),
                        NitEmisorInexistente = ContarError(grupo, "nitEmisorInexistente"),
                        NitReceptorInexistente = ContarError(grupo, "nitReceptorInexistente"),
                        IvaMalCalculado = ContarError(grupo, "ivaMalCalculado"),
                        TotalMalCalculado = ContarError(grupo, "totalMalCalculado"),
                        ReferenciaDuplicada = ContarError(grupo, "referenciaDuplicada"),
                        FacturasSinErrores = grupo.Count(factura => factura.Errores.Count == 0),
                        EmisoresDistintos = grupo
                            .Select(factura => factura.NitEmisor)
                            .Where(nit => !string.IsNullOrWhiteSpace(nit))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .Count(),
                        ReceptoresDistintos = grupo
                            .Select(factura => factura.NitReceptor)
                            .Where(nit => !string.IsNullOrWhiteSpace(nit))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .Count()
                    })
                    .ToList()
            };
        }

        private async Task<List<RegistroFacturaDto>> LeerFacturasAsync(
            CancellationToken cancellationToken)
        {
            return await almacenamiento.LeerAsync(
                ArchivoFacturas,
                new List<RegistroFacturaDto>(),
                cancellationToken);
        }

        private static bool EstaEnRango(
            DateTime fecha,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            return fecha.Date >= fechaInicio.Date && fecha.Date <= fechaFin.Date;
        }

        private static string FormatearFecha(DateTime fecha)
        {
            return fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static void AgregarIva(
            Dictionary<string, DetalleIvaDto> resumen,
            string nit,
            decimal iva,
            bool emitido)
        {
            if (string.IsNullOrWhiteSpace(nit))
            {
                return;
            }

            if (!resumen.TryGetValue(nit, out DetalleIvaDto? detalle))
            {
                detalle = new DetalleIvaDto { Nit = nit };
                resumen.Add(nit, detalle);
            }

            if (emitido)
            {
                detalle.IvaEmitido += iva;
            }
            else
            {
                detalle.IvaRecibido += iva;
            }
        }

        private static int ContarError(
            IEnumerable<RegistroFacturaDto> facturas,
            string nombreError)
        {
            return facturas.Count(factura => factura.Errores.Contains(
                nombreError,
                StringComparer.Ordinal));
        }
    }
}
