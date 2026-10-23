
using Microsoft.AspNetCore.Mvc;
using AutorizadorDTE.BackendAPI.DTOs;
using AutorizadorDTE.BackendAPI.Services;

namespace AutorizadorDTE.BackendAPI.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class RTUController : ControllerBase
    {
        private readonly RTUService rtuService;

        public RTUController(RTUService rtuService)
        {
            this.rtuService = rtuService;
        }

        [HttpPost]
        public async Task<ActionResult<RespuestaRtuDto>> ActualizarRTU(
            [FromBody] List<SolicitudRtuDto> solicitudes)
        {
            RespuestaRtuDto respuesta =
                await rtuService.ProcesarRTUAsync(solicitudes);

            return Ok(respuesta);
        }
    }
}
