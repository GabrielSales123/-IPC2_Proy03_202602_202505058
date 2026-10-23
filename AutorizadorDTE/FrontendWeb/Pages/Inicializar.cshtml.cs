using FrontendWeb.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendWeb.Pages;

public class InicializarModel(ApiService apiService) : PageModel
{
    public string? Mensaje { get; private set; }
    public bool Error { get; private set; }

    public async Task OnPostAsync()
    {
        try
        {
            ApiReply reply = await apiService.PostJsonAsync("Inicializar", "{}");
            Mensaje = reply.Content;
            Error = !reply.Success;
        }
        catch (HttpRequestException)
        {
            Mensaje = "No fue posible conectar con la API. Verifica Api:BaseUrl y que el backend esté iniciado.";
            Error = true;
        }
    }
}
