using System.Text.Json;
using FrontendWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendWeb.Pages;

public class EnviarMensajeModel(ApiService apiService) : PageModel
{
    public List<MessageResult> Resultados { get; } = new();

    public async Task<IActionResult> OnPostAsync(List<string> nombres, List<string> contenidos)
    {
        if (nombres.Count == 0 || nombres.Count != contenidos.Count)
        {
            ModelState.AddModelError(string.Empty, "Selecciona archivos JSON válidos antes de enviarlos.");
            return Page();
        }

        for (int index = 0; index < nombres.Count; index++)
        {
            string name = nombres[index];
            string json = contenidos[index];
            try
            {
                using JsonDocument document = JsonDocument.Parse(json);
                JsonElement root = document.RootElement;
                if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() == 0)
                {
                    Resultados.Add(new MessageResult(name, "No reconocido", false, "El archivo debe contener un arreglo JSON no vacío."));
                    continue;
                }

                JsonElement first = root[0];
                bool isRtu = first.ValueKind == JsonValueKind.Object &&
                    first.TryGetProperty("NIT", out _) && first.TryGetProperty("nombre", out _);
                bool isInvoice = first.ValueKind == JsonValueKind.Object &&
                    first.TryGetProperty("TIEMPO", out _) && first.TryGetProperty("REFERENCIA", out _);
                if (isRtu == isInvoice)
                {
                    Resultados.Add(new MessageResult(name, "No reconocido", false, "No se reconoce el formato del mensaje."));
                    continue;
                }

                bool mixedTypes = root.EnumerateArray().Any(row =>
                {
                    if (row.ValueKind != JsonValueKind.Object)
                        return true;
                    bool rowIsRtu = row.TryGetProperty("NIT", out _) && row.TryGetProperty("nombre", out _);
                    bool rowIsInvoice = row.TryGetProperty("TIEMPO", out _) && row.TryGetProperty("REFERENCIA", out _);
                    return rowIsRtu != isRtu || rowIsInvoice != isInvoice;
                });
                if (mixedTypes)
                {
                    Resultados.Add(new MessageResult(name, "No reconocido", false, "El archivo contiene registros mezclados o con un formato no reconocido."));
                    continue;
                }

                string type = isRtu ? "RTU" : "Facturas";
                string endpoint = isRtu ? "RTU" : "Procesar";
                ApiReply reply;
                try
                {
                    reply = await apiService.PostJsonAsync(endpoint, json);
                }
                catch (HttpRequestException)
                {
                    Resultados.Add(new MessageResult(name, type, false, "No fue posible conectar con la API. Verifica Api:BaseUrl y que el backend esté iniciado."));
                    continue;
                }

                int? approved = null;
                int? rejected = null;
                if (reply.Success && !isRtu)
                {
                    using JsonDocument response = JsonDocument.Parse(reply.Content);
                    if (response.RootElement.TryGetProperty("resultadoAutorizaciones", out JsonElement result))
                    {
                        if (result.TryGetProperty("cantidadAutorizacionesAprobadas", out JsonElement approvedValue))
                            approved = approvedValue.GetInt32();
                        if (result.TryGetProperty("cantidadAutorizacionesRechazadas", out JsonElement rejectedValue))
                            rejected = rejectedValue.GetInt32();
                    }
                }

                Resultados.Add(new MessageResult(name, type, reply.Success, reply.Content, approved, rejected));
            }
            catch (JsonException)
            {
                Resultados.Add(new MessageResult(name, "No reconocido", false, "El archivo contiene JSON inválido o la API devolvió una respuesta JSON no válida."));
            }
        }

        return Page();
    }
}

public record MessageResult(string Nombre, string Tipo, bool Success, string Content, int? Aprobadas = null, int? Rechazadas = null);
