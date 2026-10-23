using System.Text.Json.Serialization;

namespace FrontendWeb.Models;

public class ResumenRangoDto
{
    [JsonInclude, JsonPropertyName("fechaInicio")]
    private string fechaInicio = "";
    [JsonInclude, JsonPropertyName("fechaFin")]
    private string fechaFin = "";
    [JsonInclude, JsonPropertyName("modo")]
    private string modo = "";
    [JsonInclude, JsonPropertyName("detalle")]
    private List<DetalleResumenRangoDto> detalle = new();
    [JsonInclude, JsonPropertyName("totalRango")]
    private decimal totalRango;

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

    public void setModo(string value)
    {
        modo = value;
    }

    public string getModo()
    {
        return modo;
    }

    public void setDetalle(List<DetalleResumenRangoDto> value)
    {
        detalle = value;
    }

    public List<DetalleResumenRangoDto> getDetalle()
    {
        return detalle;
    }

    public void setTotalRango(decimal value)
    {
        totalRango = value;
    }

    public decimal getTotalRango()
    {
        return totalRango;
    }
}
