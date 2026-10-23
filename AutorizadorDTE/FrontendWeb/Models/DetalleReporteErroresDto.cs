using System.Text.Json.Serialization;

namespace FrontendWeb.Models;

public class DetalleReporteErroresDto
{
    [JsonInclude, JsonPropertyName("fecha")]
    private string fecha = "";
    [JsonInclude, JsonPropertyName("facturasRecibidas")]
    private int facturasRecibidas;
    [JsonInclude, JsonPropertyName("nitEmisorInvalido")]
    private int nitEmisorInvalido;
    [JsonInclude, JsonPropertyName("nitReceptorInvalido")]
    private int nitReceptorInvalido;
    [JsonInclude, JsonPropertyName("nitEmisorInexistente")]
    private int nitEmisorInexistente;
    [JsonInclude, JsonPropertyName("nitReceptorInexistente")]
    private int nitReceptorInexistente;
    [JsonInclude, JsonPropertyName("ivaMalCalculado")]
    private int ivaMalCalculado;
    [JsonInclude, JsonPropertyName("totalMalCalculado")]
    private int totalMalCalculado;
    [JsonInclude, JsonPropertyName("referenciaDuplicada")]
    private int referenciaDuplicada;
    [JsonInclude, JsonPropertyName("facturasSinErrores")]
    private int facturasSinErrores;
    [JsonInclude, JsonPropertyName("emisoresDistintos")]
    private int emisoresDistintos;
    [JsonInclude, JsonPropertyName("receptoresDistintos")]
    private int receptoresDistintos;

    public void setFecha(string value)
    {
        fecha = value;
    }

    public string getFecha()
    {
        return fecha;
    }

    public void setFacturasRecibidas(int value)
    {
        facturasRecibidas = value;
    }

    public int getFacturasRecibidas()
    {
        return facturasRecibidas;
    }

    public void setNitEmisorInvalido(int value)
    {
        nitEmisorInvalido = value;
    }

    public int getNitEmisorInvalido()
    {
        return nitEmisorInvalido;
    }

    public void setNitReceptorInvalido(int value)
    {
        nitReceptorInvalido = value;
    }

    public int getNitReceptorInvalido()
    {
        return nitReceptorInvalido;
    }

    public void setNitEmisorInexistente(int value)
    {
        nitEmisorInexistente = value;
    }

    public int getNitEmisorInexistente()
    {
        return nitEmisorInexistente;
    }

    public void setNitReceptorInexistente(int value)
    {
        nitReceptorInexistente = value;
    }

    public int getNitReceptorInexistente()
    {
        return nitReceptorInexistente;
    }

    public void setIvaMalCalculado(int value)
    {
        ivaMalCalculado = value;
    }

    public int getIvaMalCalculado()
    {
        return ivaMalCalculado;
    }

    public void setTotalMalCalculado(int value)
    {
        totalMalCalculado = value;
    }

    public int getTotalMalCalculado()
    {
        return totalMalCalculado;
    }

    public void setReferenciaDuplicada(int value)
    {
        referenciaDuplicada = value;
    }

    public int getReferenciaDuplicada()
    {
        return referenciaDuplicada;
    }

    public void setFacturasSinErrores(int value)
    {
        facturasSinErrores = value;
    }

    public int getFacturasSinErrores()
    {
        return facturasSinErrores;
    }

    public void setEmisoresDistintos(int value)
    {
        emisoresDistintos = value;
    }

    public int getEmisoresDistintos()
    {
        return emisoresDistintos;
    }

    public void setReceptoresDistintos(int value)
    {
        receptoresDistintos = value;
    }

    public int getReceptoresDistintos()
    {
        return receptoresDistintos;
    }
}
