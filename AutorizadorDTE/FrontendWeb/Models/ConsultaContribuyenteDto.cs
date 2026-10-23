using System.Text.Json.Serialization;

namespace FrontendWeb.Models;

public class ConsultaContribuyenteDto
{
    [JsonInclude, JsonPropertyName("nit")]
    private string nit = "";
    [JsonInclude, JsonPropertyName("tipo")]
    private string tipo = "";
    [JsonInclude, JsonPropertyName("fechaInicio")]
    private string fechaInicio = "";
    [JsonInclude, JsonPropertyName("fechaFin")]
    private string fechaFin = "";
    [JsonInclude, JsonPropertyName("detalle")]
    private List<DetalleContribuyenteDto> detalle = new();
    [JsonInclude, JsonPropertyName("totalFacturas")]
    private int totalFacturas;
    [JsonInclude, JsonPropertyName("montoTotalRango")]
    private decimal montoTotalRango;

    public void setNit(string value)
    {
        nit = value;
    }

    public string getNit()
    {
        return nit;
    }

    public void setTipo(string value)
    {
        tipo = value;
    }

    public string getTipo()
    {
        return tipo;
    }

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

    public void setDetalle(List<DetalleContribuyenteDto> value)
    {
        detalle = value;
    }

    public List<DetalleContribuyenteDto> getDetalle()
    {
        return detalle;
    }

    public void setTotalFacturas(int value)
    {
        totalFacturas = value;
    }

    public int getTotalFacturas()
    {
        return totalFacturas;
    }

    public void setMontoTotalRango(decimal value)
    {
        montoTotalRango = value;
    }

    public decimal getMontoTotalRango()
    {
        return montoTotalRango;
    }
}
