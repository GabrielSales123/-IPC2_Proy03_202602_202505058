using AutorizadorDTE.BackendAPI.DTOs;
using AutorizadorDTE.BackendAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutorizadorDTE.BackendAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly ConsultasService consultasService;

        public ConsultasController(ConsultasService consultasService)
        {
            this.consultasService = consultasService;
        }

        [HttpGet("Contribuyente")]
        public async Task<ActionResult<ConsultaContribuyenteDto>> ConsultarContribuyente(
            [FromQuery] string nit,
            [FromQuery] string tipo,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(nit))
            {
                return BadRequest(new { error = "El NIT es obligatorio." });
            }

            if (string.IsNullOrWhiteSpace(tipo) ||
                (!tipo.Equals("emitidas", StringComparison.OrdinalIgnoreCase) &&
                 !tipo.Equals("recibidas", StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(new { error = "El tipo debe ser emitidas o recibidas." });
            }

            if (!FechasValidas(fechaInicio, fechaFin))
            {
                return BadRequest(new
                {
                    error = "Debe indicar un rango de fechas válido y la fecha inicial no puede ser posterior a la final."
                });
            }

            ConsultaContribuyenteDto respuesta =
                await consultasService.ConsultarContribuyenteAsync(
                    nit,
                    tipo,
                    fechaInicio!.Value,
                    fechaFin!.Value,
                    cancellationToken);

            return Ok(respuesta);
        }

        [HttpGet("ResumenIva")]
        public async Task<ActionResult<ResumenIvaDto>> ConsultarResumenIva(
            [FromQuery] DateTime? fecha,
            CancellationToken cancellationToken)
        {
            if (fecha is null)
            {
                return BadRequest(new { error = "La fecha es obligatoria." });
            }

            ResumenIvaDto respuesta =
                await consultasService.ConsultarResumenIvaAsync(
                    fecha.Value,
                    cancellationToken);

            return Ok(respuesta);
        }

        [HttpGet("ResumenRango")]
        public async Task<ActionResult<ResumenRangoDto>> ConsultarResumenRango(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] string modo,
            CancellationToken cancellationToken)
        {
            if (!FechasValidas(fechaInicio, fechaFin))
            {
                return BadRequest(new
                {
                    error = "Debe indicar un rango de fechas válido y la fecha inicial no puede ser posterior a la final."
                });
            }

            if (string.IsNullOrWhiteSpace(modo) ||
                (!modo.Equals("total", StringComparison.OrdinalIgnoreCase) &&
                 !modo.Equals("sinIva", StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(new { error = "El modo debe ser total o sinIva." });
            }

            ResumenRangoDto respuesta =
                await consultasService.ConsultarResumenRangoAsync(
                    fechaInicio!.Value,
                    fechaFin!.Value,
                    modo,
                    cancellationToken);

            return Ok(respuesta);
        }

        [HttpGet("ReporteErrores")]
        public async Task<ActionResult<ReporteErroresDto>> ConsultarReporteErrores(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            CancellationToken cancellationToken)
        {
            if (!FechasValidas(fechaInicio, fechaFin))
            {
                return BadRequest(new
                {
                    error = "Debe indicar un rango de fechas válido y la fecha inicial no puede ser posterior a la final."
                });
            }

            ReporteErroresDto respuesta =
                await consultasService.ConsultarReporteErroresAsync(
                    fechaInicio!.Value,
                    fechaFin!.Value,
                    cancellationToken);

            return Ok(respuesta);
        }

        private static bool FechasValidas(DateTime? fechaInicio, DateTime? fechaFin)
        {
            return fechaInicio.HasValue &&
                fechaFin.HasValue &&
                fechaInicio.Value.Date <= fechaFin.Value.Date;
        }
    }
}
