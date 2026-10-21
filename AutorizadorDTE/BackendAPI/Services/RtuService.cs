
using AutorizadorDTE.BackendAPI.DTOs;
using AutorizadorDTE.BackendAPI.Models;

namespace AutorizadorDTE.BackendAPI.Services
{
    public class RTUService
    {
        private readonly AlmacenamientoService almacenamiento;

        private const string ArchivoRTU = "rtu.json";

        public RTUService(AlmacenamientoService almacenamiento)
        {
            this.almacenamiento = almacenamiento;
        }

        public async Task<RespuestaRtuDto> ProcesarRTUAsync(
            List<SolicitudRtuDto> solicitudes)
        {
            RespuestaRtuDto respuesta = new RespuestaRtuDto();

            List<SolicitudRtuDto> datosRTU =
                await almacenamiento.LeerAsync(
                    ArchivoRTU,
                    new List<SolicitudRtuDto>()
                );

            Dictionary<string, Contribuyente> contribuyentes =
                new Dictionary<string, Contribuyente>();

            // Cargar los contribuyentes que ya existen.
            foreach (SolicitudRtuDto dato in datosRTU)
            {
                string nit = (dato.NIT ?? "")
                    .Trim()
                    .ToUpperInvariant();

                if (ValidadorNIT.Validar(nit))
                {
                    Contribuyente contribuyente = new Contribuyente(
                        dato.nombre,
                        nit
                    );

                    contribuyentes[nit] = contribuyente;
                }
            }

            // Procesar los contribuyentes recibidos.
            foreach (SolicitudRtuDto solicitud in solicitudes)
            {
                string nit = (solicitud.NIT ?? "")
                    .Trim()
                    .ToUpperInvariant();

                if (!ValidadorNIT.Validar(nit))
                {
                    respuesta.NITS_INVALIDOS++;
                    continue;
                }

                if (contribuyentes.TryGetValue(
                    nit,
                    out Contribuyente? existente))
                {
                    // Actualizar el nombre del contribuyente.
                    existente.setNombre(solicitud.nombre);

                    respuesta.CONTRIBUYENTES_ACTUALIZADOS++;
                }
                else
                {
                    // Registrar un contribuyente nuevo.
                    Contribuyente nuevo = new Contribuyente(
                        solicitud.nombre,
                        nit
                    );

                    contribuyentes.Add(nit, nuevo);

                    respuesta.CONTRIBUYENTES_NUEVOS++;
                }
            }

            // Convertir los datos al DTO para guardarlos como JSON.
            List<SolicitudRtuDto> datosGuardar =
                new List<SolicitudRtuDto>();

            foreach (KeyValuePair<string, Contribuyente> elemento
                in contribuyentes)
            {
                datosGuardar.Add(new SolicitudRtuDto
                {
                    NIT = elemento.Key,
                    nombre = elemento.Value.getNombre() ?? ""
                });
            }

            await almacenamiento.GuardarAsync(
                ArchivoRTU,
                datosGuardar
            );

            return respuesta;
        }

        public async Task<bool> ExisteContribuyenteAsync(string nit)
        {
            nit = (nit ?? "").Trim().ToUpperInvariant();

            if (!ValidadorNIT.Validar(nit))
            {
                return false;
            }

            List<SolicitudRtuDto> datosRTU =
                await almacenamiento.LeerAsync(
                    ArchivoRTU,
                    new List<SolicitudRtuDto>()
                );

            return datosRTU.Any(dato =>
                string.Equals(
                    (dato.NIT ?? "").Trim(),
                    nit,
                    StringComparison.OrdinalIgnoreCase
                )
            );
        }
    }
}
