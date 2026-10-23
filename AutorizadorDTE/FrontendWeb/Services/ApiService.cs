using System.Text;
using System.Text.Json;

namespace FrontendWeb.Services;

public class ApiService
{
    public static JsonSerializerOptions JsonOptions { get; } = new(JsonSerializerDefaults.Web);
    private readonly HttpClient httpClient;

    public ApiService(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        string baseUrl = configuration["Api:BaseUrl"]
            ?? throw new InvalidOperationException("Falta configurar Api:BaseUrl.");
        this.httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<ApiReply> GetAsync(string path)
    {
        using HttpResponseMessage response = await httpClient.GetAsync(path);
        return await CreateReplyAsync(response);
    }

    public async Task<ApiReply> PostJsonAsync(string path, string json)
    {
        using StringContent content = new(json, Encoding.UTF8, "application/json");
        using HttpResponseMessage response = await httpClient.PostAsync(path, content);
        return await CreateReplyAsync(response);
    }

    private static async Task<ApiReply> CreateReplyAsync(HttpResponseMessage response)
    {
        string body = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            return new ApiReply(true, body);
        }

        string message = $"La API respondió con {(int)response.StatusCode} ({response.ReasonPhrase}).";
        if (!string.IsNullOrWhiteSpace(body))
        {
            try
            {
                using JsonDocument document = JsonDocument.Parse(body);
                JsonElement root = document.RootElement;
                if (root.ValueKind == JsonValueKind.Object)
                {
                    foreach (string property in new[] { "error", "message", "detail", "title" })
                    {
                        if (root.TryGetProperty(property, out JsonElement value) &&
                            value.ValueKind == JsonValueKind.String)
                        {
                            message = value.GetString() ?? message;
                            break;
                        }
                    }
                }
            }
            catch (JsonException)
            {
                message = $"{message} {body}";
            }
        }

        return new ApiReply(false, message);
    }
}

public record ApiReply(bool Success, string Content);
