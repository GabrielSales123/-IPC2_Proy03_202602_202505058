
using System.Text.Json;

namespace AutorizadorDTE.BackendAPI.Services
{
    public class AlmacenamientoService
    {
        private readonly string _rutaDatos;

        private readonly JsonSerializerOptions _opcionesJson =
            new JsonSerializerOptions
            {
                WriteIndented = true
            };

        public AlmacenamientoService(
            Microsoft.AspNetCore.Hosting.IWebHostEnvironment entorno)
        {
            _rutaDatos = Path.Combine(
                entorno.ContentRootPath,
                "Data"
            );

            Directory.CreateDirectory(_rutaDatos);
        }

        public async Task<T> LeerAsync<T>(
            string nombreArchivo,
            T valorPorDefecto,
            CancellationToken cancellationToken = default)
        {
            string rutaArchivo = Path.Combine(
                _rutaDatos,
                nombreArchivo
            );

            if (!File.Exists(rutaArchivo))
            {
                return valorPorDefecto;
            }

            string contenido = await File.ReadAllTextAsync(
                rutaArchivo,
                cancellationToken
            );

            T? datos = JsonSerializer.Deserialize<T>(
                contenido,
                _opcionesJson
            );

            return datos is not null
                ? datos
                : valorPorDefecto;
        }

        public async Task GuardarAsync<T>(
            string nombreArchivo,
            T datos)
        {
            string rutaArchivo = Path.Combine(
                _rutaDatos,
                nombreArchivo
            );

            string contenido = JsonSerializer.Serialize(
                datos,
                _opcionesJson
            );

            await File.WriteAllTextAsync(
                rutaArchivo,
                contenido
            );
        }
    }
}
