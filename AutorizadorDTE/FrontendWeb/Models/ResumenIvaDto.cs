using System.Text.Json.Serialization;

namespace FrontendWeb.Models;

public class ResumenIvaDto
{
    [JsonInclude, JsonPropertyName("fecha")]
    private string fecha = "";
    [JsonInclude, JsonPropertyName("detalle")]
    private List<DetalleIvaDto> detalle = new();

    public void setFecha(string value)
    {
        fecha = value;
    }

    public string getFecha()
    {
        return fecha;
    }

    public void setDetalle(List<DetalleIvaDto> value)
    {
        detalle = value;
    }

    public List<DetalleIvaDto> getDetalle()
    {
        return detalle;
    }
}
