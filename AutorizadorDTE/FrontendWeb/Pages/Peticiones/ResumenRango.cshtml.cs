using System.Globalization;
using System.Text.Json;
using FrontendWeb.Models;
using FrontendWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendWeb.Pages.Peticiones;

public class ResumenRangoModel(ApiService apiService) : PageModel
{
    [BindProperty] public DateTime? FechaInicio { get; set; }
    [BindProperty] public DateTime? FechaFin { get; set; }
    [BindProperty] public string Modo { get; set; } = "total";
    public ResumenRangoDto? Resultado { get; private set; }
    public string? Error { get; private set; }

    public async Task OnPostAsync()
    {
        if (FechaInicio is null || FechaFin is null || FechaInicio.Value.Date > FechaFin.Value.Date)
        {
            Error = "Indica un rango de fechas válido.";
            return;
        }

        string start = FechaInicio.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        string end = FechaFin.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        string query = $"fechaInicio={start}&fechaFin={end}&modo={Uri.EscapeDataString(Modo)}";
        try
        {
            ApiReply reply = await apiService.GetAsync($"Consultas/ResumenRango?{query}");
            if (!reply.Success) { Error = reply.Content; return; }
            Resultado = JsonSerializer.Deserialize<ResumenRangoDto>(reply.Content, ApiService.JsonOptions);
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
