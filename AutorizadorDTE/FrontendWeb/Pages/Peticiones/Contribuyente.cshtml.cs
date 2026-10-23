using System.Globalization;
using System.Text.Json;
using FrontendWeb.Models;
using FrontendWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendWeb.Pages.Peticiones;

public class ContribuyenteModel(ApiService apiService) : PageModel
{
    [BindProperty] public string Nit { get; set; } = "";
    [BindProperty] public string Tipo { get; set; } = "emitidas";
    [BindProperty] public DateTime? FechaInicio { get; set; }
    [BindProperty] public DateTime? FechaFin { get; set; }
    public ConsultaContribuyenteDto? Resultado { get; private set; }
    public string? Error { get; private set; }

    public async Task OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Nit) || FechaInicio is null || FechaFin is null ||
            FechaInicio.Value.Date > FechaFin.Value.Date)
        {
            Error = "Indica un NIT y un rango de fechas válido.";
            return;
        }

        string query = $"nit={Uri.EscapeDataString(Nit)}&tipo={Uri.EscapeDataString(Tipo)}" +
            $"&fechaInicio={FechaInicio.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}" +
            $"&fechaFin={FechaFin.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";
        try
        {
            ApiReply reply = await apiService.GetAsync($"Consultas/Contribuyente?{query}");
            if (!reply.Success) { Error = reply.Content; return; }
            Resultado = JsonSerializer.Deserialize<ConsultaContribuyenteDto>(reply.Content, ApiService.JsonOptions);
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
}
