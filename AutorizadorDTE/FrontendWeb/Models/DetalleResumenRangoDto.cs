using System.Text.Json.Serialization;

namespace FrontendWeb.Models;

public class DetalleResumenRangoDto
{
    [JsonInclude, JsonPropertyName("fecha")]
    private string fecha = "";
    [JsonInclude, JsonPropertyName("monto")]
    private decimal monto;

    public void setFecha(string value)
    {
        fecha = value;
    }

    public string getFecha()
    {
        return fecha;
    }

    public void setMonto(decimal value)
    {
        monto = value;
    }

    public decimal getMonto()
    {
        return monto;
    }
}
