using System.Globalization;
using System.Text.Json;
using FrontendWeb.Models;
using FrontendWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendWeb.Pages.Peticiones;

public class ResumenIvaModel(ApiService apiService) : PageModel
{
    [BindProperty] public DateTime? Fecha { get; set; }
    public ResumenIvaDto? Resultado { get; private set; }
    public string? Error { get; private set; }

    public async Task OnPostAsync()
    {
        if (Fecha is null) { Error = "Selecciona una fecha."; return; }

        string date = Fecha.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        try
        {
            ApiReply reply = await apiService.GetAsync($"Consultas/ResumenIva?fecha={date}");
            if (!reply.Success) { Error = reply.Content; return; }
            Resultado = JsonSerializer.Deserialize<ResumenIvaDto>(reply.Content, ApiService.JsonOptions);
            Error = Resultado is null ? "La API devolvió una respuesta vacía." : null;
        }
        catch (HttpRequestException)
        {
            Error = "No fue posible conectar con la API. Verifica Api:BaseUrl y que el backend esté iniciado.";
        }
        catch (JsonException)
        {
            Error = "La API devolvió una respuesta JSON que no se pudo leer.";
        }
    }

    public string GetWidth(decimal value, decimal maximum) =>
        maximum <= 0 ? "0%" : $"{Math.Max(2, (double)(value / maximum * 100)):0.##}%";
}
