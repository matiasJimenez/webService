using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GestionVentasServicios.Services.Models;
using Microsoft.Extensions.Options;

namespace GestionVentasServicios.Services
{
    public class OpenAiAnswerFormatter : IAiAnswerFormatter
    {
        private readonly HttpClient _httpClient;
        private readonly OpenAiOptions _options;

        public OpenAiAnswerFormatter(HttpClient httpClient, IOptions<OpenAiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<string?> BuildNaturalResponseAsync(string userQuery, string jsonData, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                throw new InvalidOperationException("Configura OpenAI:ApiKey en appsettings.json o como variable de entorno OPENAI__APIKEY.");
            }

            var endpoint = $"{_options.BaseUrl?.TrimEnd('/')}/chat/completions";
            var systemPrompt = @"Eres un asistente que resume resultados de base de datos en español, tono claro y conciso. Responde en máximo 3 frases. No inventes datos, solo usa los resultados JSON provistos. Si no hay datos, dilo.";

            var payload = new
            {
                model = string.IsNullOrWhiteSpace(_options.Model) ? "gpt-4o-mini" : _options.Model,
                messages = new object[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = $"Consulta del usuario: {userQuery}\nResultados JSON: {jsonData}" }
                },
                temperature = 0.2
            };

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            var completion = JsonDocument.Parse(body);
            var content = completion.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
            return content;
        }
    }
}
