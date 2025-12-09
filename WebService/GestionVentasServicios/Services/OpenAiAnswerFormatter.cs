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
        private readonly ILlmChatClient _llmClient;

        public OpenAiAnswerFormatter(ILlmChatClient llmClient)
        {
            _llmClient = llmClient;
        }

        public async Task<string?> BuildNaturalResponseAsync(string userQuery, string jsonData, CancellationToken cancellationToken = default)
        {
            var systemPrompt = @"Eres un asistente que resume resultados de base de datos en español, tono claro y conciso. Responde en máximo 3 frases. No inventes datos, solo usa los resultados JSON provistos. Si no hay datos, dilo.";
            return await _llmClient.SendAsync(new[]
            {
                ("system", systemPrompt),
                ("user", $"Consulta del usuario: {userQuery}\nResultados JSON: {jsonData}")
            }, jsonResponse: false, cancellationToken);
        }
    }
}
