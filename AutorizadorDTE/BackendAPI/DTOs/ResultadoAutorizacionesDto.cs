using System.Text.Json.Serialization;

namespace AutorizadorDTE.BackendAPI.DTOs
{
    public class ResultadoAutorizacionesDto
    {
        [JsonPropertyName("cantidadAutorizacionesAprobadas")]
        public int CantidadAutorizacionesAprobadas { get; set; }
        [JsonPropertyName("cantidadAutorizacionesRechazadasPorNitEmisorInexistente")]
        public int CantidadAutorizacionesRechazadasPorNitEmisorInexistente { get; set; }
        [JsonPropertyName("cantidadAutorizacionesRechazadasPorNitReceptorInexistente")]
        public int CantidadAutorizacionesRechazadasPorNitReceptorInexistente { get; set; }
        [JsonPropertyName("cantidadAutorizacionesRechazadasPorNitEmisorInvalido")]
        public int CantidadAutorizacionesRechazadasPorNitEmisorInvalido { get; set; }
        [JsonPropertyName("cantidadAutorizacionesRechazadasPorNitReceptorInvalido")]
        public int CantidadAutorizacionesRechazadasPorNitReceptorInvalido { get; set; }
        [JsonPropertyName("cantidadAutorizacionesRechazadasPorIvaMalCalculado")]
        public int CantidadAutorizacionesRechazadasPorIvaMalCalculado { get; set; }
        [JsonPropertyName("cantidadAutorizacionesRechazadasPorTotalMalCalculado")]
        public int CantidadAutorizacionesRechazadasPorTotalMalCalculado { get; set; }
        [JsonPropertyName("cantidadAutorizacionesRechazadasPorReferenciaDuplicada")]
        public int CantidadAutorizacionesRechazadasPorReferenciaDuplicada { get; set; }
        [JsonPropertyName("cantidadAutorizacionesRechazadasPorReferenciaInvalida")]
        public int CantidadAutorizacionesRechazadasPorReferenciaInvalida { get; set; }
        [JsonPropertyName("cantidadAutorizacionesRechazadas")]
        public int CantidadAutorizacionesRechazadas { get; set; }
        [JsonPropertyName("cantidadDescartadasPorFormato")]
        public int CantidadDescartadasPorFormato { get; set; }
        [JsonPropertyName("detalleAprobaciones")]
        public List<DetalleAprobacionDto> DetalleAprobaciones { get; set; } = new();
    }

    public class RespuestaAutorizacionesDto
    {
        [JsonPropertyName("resultadoAutorizaciones")]
        public ResultadoAutorizacionesDto ResultadoAutorizaciones { get; set; } = new();
    }
}