using System.Text.Json.Serialization;

namespace FrontendWeb.Models;

public class ReporteErroresDto
{
    [JsonInclude, JsonPropertyName("fechaInicio")]
    private string fechaInicio = "";
    [JsonInclude, JsonPropertyName("fechaFin")]
    private string fechaFin = "";
    [JsonInclude, JsonPropertyName("detalle")]
    private List<DetalleReporteErroresDto> detalle = new();

    public void setFechaInicio(string value)
    {
        fechaInicio = value;
    }

    public string getFechaInicio()
    {
        return fechaInicio;
    }

    public void setFechaFin(string value)
    {
        fechaFin = value;
    }

    public string getFechaFin()
    {
        return fechaFin;
    }

    public void setDetalle(List<DetalleReporteErroresDto> value)
    {
        detalle = value;
    }

    public List<DetalleReporteErroresDto> getDetalle()
    {
        return detalle;
    }
}
