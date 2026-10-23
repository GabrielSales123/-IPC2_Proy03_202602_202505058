using System.Text.Json.Serialization;

namespace FrontendWeb.Models;

public class DetalleIvaDto
{
    [JsonInclude, JsonPropertyName("nit")]
    private string nit = "";
    [JsonInclude, JsonPropertyName("ivaEmitido")]
    private decimal ivaEmitido;
    [JsonInclude, JsonPropertyName("ivaRecibido")]
    private decimal ivaRecibido;

    public void setNit(string value)
    {
        nit = value;
    }

    public string getNit()
    {
        return nit;
    }

    public void setIvaEmitido(decimal value)
    {
        ivaEmitido = value;
    }

    public decimal getIvaEmitido()
    {
        return ivaEmitido;
    }

    public void setIvaRecibido(decimal value)
    {
        ivaRecibido = value;
    }

    public decimal getIvaRecibido()
    {
        return ivaRecibido;
    }
}
