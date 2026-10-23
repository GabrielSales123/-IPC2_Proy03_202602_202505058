using AutorizadorDTE.BackendAPI.DTOs;
using AutorizadorDTE.BackendAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutorizadorDTE.BackendAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class FacturaController : ControllerBase
    {
        private readonly FacturaService facturaService;

        public FacturaController(FacturaService facturaService)
        {
            this.facturaService = facturaService;
        }

        [HttpPost("Procesar")]
        public async Task<ActionResult<RespuestaAutorizacionesDto>> Procesar(
            [FromBody] List<SolicitudFacturaDto> solicitudes)
        {
            if (solicitudes is null || solicitudes.Count == 0)
            {
                return BadRequest(new
                {
                    error = "El mensaje debe contener un arreglo no vacío de solicitudes de factura."
                });
            }

            return Ok(await facturaService.ProcesarAsync(solicitudes));
        }

        [HttpPost("Inicializar")]
        public async Task<IActionResult> Inicializar()
        {
            await facturaService.InicializarAsync();
            return Ok(new { mensaje = "Facturas inicializadas correctamente." });
        }
    }
}
