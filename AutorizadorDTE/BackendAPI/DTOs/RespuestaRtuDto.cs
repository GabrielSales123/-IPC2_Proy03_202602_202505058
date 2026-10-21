
using System.Text.Json.Serialization;

namespace AutorizadorDTE.BackendAPI.DTOs
{
    public class RespuestaRtuDto
    {
        [JsonPropertyName("CONTRIBUYENTES_NUEVOS")]
        public int CONTRIBUYENTES_NUEVOS { get; set; }

        [JsonPropertyName("CONTRIBUYENTES_ACTUALIZADOS")]
        public int CONTRIBUYENTES_ACTUALIZADOS { get; set; }

        [JsonPropertyName("NITS_INVALIDOS")]
        public int NITS_INVALIDOS { get; set; }
    }
}
