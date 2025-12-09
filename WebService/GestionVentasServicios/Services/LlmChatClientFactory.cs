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
    /// <summary>
    /// Cliente unificado para OpenAI y Ollama (chat). Soporta response_format json cuando jsonResponse=true.
    /// </summary>
    public class LlmChatClientFactory : ILlmChatClient
    {
        private readonly HttpClient _httpClient;
        private readonly LlmOptions _options;

        public LlmChatClientFactory(HttpClient httpClient, IOptions<LlmOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<string?> SendAsync(System.Collections.Generic.IEnumerable<(string role, string content)> messages, bool jsonResponse, CancellationToken cancellationToken = default)
        {
            var provider = (_options.Provider ?? "openai").Trim().ToLowerInvariant();
            return provider switch
            {
                "ollama" => await SendOllamaAsync(messages, jsonResponse, cancellationToken),
                _ => await SendOpenAiAsync(messages, jsonResponse, cancellationToken)
            };
        }

        private async Task<string?> SendOpenAiAsync(System.Collections.Generic.IEnumerable<(string role, string content)> messages, bool jsonResponse, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                throw new InvalidOperationException("Configura Llm:ApiKey para usar OpenAI.");
            }

            var endpoint = $"{_options.BaseUrl?.TrimEnd('/')}/chat/completions";
            var payload = new
            {
                model = string.IsNullOrWhiteSpace(_options.Model) ? "gpt-4o-mini" : _options.Model,
                messages = BuildMessages(messages),
                temperature = 0,
                response_format = jsonResponse ? new { type = "json_object" } : null
            };

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(JsonSerializer.Serialize(payload, new JsonSerializerOptions { IgnoreNullValues = true }), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(body);
            return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }

        private async Task<string?> SendOllamaAsync(System.Collections.Generic.IEnumerable<(string role, string content)> messages, bool jsonResponse, CancellationToken cancellationToken)
        {
            var endpoint = $"{_options.BaseUrl?.TrimEnd('/')}/api/chat";
            var payload = new
            {
                model = string.IsNullOrWhiteSpace(_options.Model) ? "llama3" : _options.Model,
                messages = BuildMessages(messages),
                stream = false,
                format = jsonResponse ? "json" : null
            };

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(JsonSerializer.Serialize(payload, new JsonSerializerOptions { IgnoreNullValues = true }), Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(body);
            // Ollama responde {"message": {"content": "..."}}
            return doc.RootElement.GetProperty("message").GetProperty("content").GetString();
        }

        private static object[] BuildMessages(System.Collections.Generic.IEnumerable<(string role, string content)> messages)
        {
            var list = new System.Collections.Generic.List<object>();
            foreach (var (role, content) in messages)
            {
                list.Add(new { role, content });
            }
            return list.ToArray();
        }
    }
}
