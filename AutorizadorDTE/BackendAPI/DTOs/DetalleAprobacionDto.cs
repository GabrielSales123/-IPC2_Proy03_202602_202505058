using System.Text.Json.Serialization;

namespace AutorizadorDTE.BackendAPI.DTOs
{
    public class DetalleAprobacionDto
    {
        [JsonPropertyName("referencia")]
        public string Referencia { get; set; } = string.Empty;

        [JsonPropertyName("codigoAprobacion")]
        public long CodigoAprobacion { get; set; }
    }
}