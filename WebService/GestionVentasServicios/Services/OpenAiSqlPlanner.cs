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
    public class OpenAiSqlPlanner : IAiSqlPlanner
    {
        private readonly ILlmChatClient _llmClient;

        public OpenAiSqlPlanner(ILlmChatClient llmClient)
        {
            _llmClient = llmClient;
        }

        public async Task<AiQueryPlan?> BuildPlanAsync(string naturalLanguageQuery, CancellationToken cancellationToken = default)
        {
            var systemPrompt = BuildSystemPrompt();
            var content = await _llmClient.SendAsync(new[]
            {
                ("system", systemPrompt),
                ("user", naturalLanguageQuery)
            }, jsonResponse: true, cancellationToken);

            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            var plan = JsonSerializer.Deserialize<AiQueryPlan>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return plan;
        }

        private static string BuildSystemPrompt()
        {
            return @"Eres un planificador de consultas SQL seguro para un API en español.
Devuelves solo JSON con la estructura: { ""entity"":""clientes|usuarios|pagos|planVentas"", ""select"":[], ""filters"":[{""entity"":""(opcional)"",""field"":"""",""operator"":""equals|contains|gt|lt"",""value"":""""}], ""limit"":50 }
Restricciones:
- Solo usa las entidades y campos permitidos.
- No incluyas deletes ni updates ni writes, solo consultas de lectura.
- El campo limit máximo es 200.
- Se permiten joins SOLO cuando entity=""pagos""; puedes referenciar campos de clientes/usuarios/planVentas en select/filters usando el prefijo de entidad (ej: ""clientes.Nombre"", ""usuarios.Email"", ""planVentas.Nombre"").
Campos permitidos por entidad:
clientes: Id, Nombre, Apellido, Email, FechaAlta, TipoDocumento, NumeroDocumento, Telefono, Direccion
usuarios: Id, Nombre, Apellido, Email, FechaAlta, Rol, FechaUltimoAcceso, Estado
pagos: Id, ClientId, FechaPago, FechaAlta, Monto, NumeroCuota, PlanVentaId, UsuarioId, MetodoPagoId, Observaciones, Estado
planVentas: Id, Nombre, Descripcion, CantidadCuotas, FechaAlta
operator ""contains"" solo para textos; gt/lt para números o fechas.
Si la consulta es ambigua, devuelve la opción más probable y un limit de 20.";
        }
    }
}
