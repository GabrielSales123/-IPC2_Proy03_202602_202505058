using System.Text.Json.Serialization;

namespace FrontendWeb.Models;

public class DetalleContribuyenteDto
{
    [JsonInclude, JsonPropertyName("fecha")]
    private string fecha = "";
    [JsonInclude, JsonPropertyName("cantidadFacturas")]
    private int cantidadFacturas;
    [JsonInclude, JsonPropertyName("montoTotal")]
    private decimal montoTotal;

    public void setFecha(string value)
    {
        fecha = value;
    }

    public string getFecha()
    {
        return fecha;
    }

    public void setCantidadFacturas(int value)
    {
        cantidadFacturas = value;
    }

    public int getCantidadFacturas()
    {
        return cantidadFacturas;
    }

    public void setMontoTotal(decimal value)
    {
        montoTotal = value;
    }

    public decimal getMontoTotal()
    {
        return montoTotal;
    }
}
