using System.Threading;
using System.Threading.Tasks;
using GestionVentasServicios.DTO.IA;
using GestionVentasServicios.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionVentasServicios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IAController : ControllerBase
    {
        private readonly IAiQueryService _aiQueryService;

        public IAController(IAiQueryService aiQueryService)
        {
            _aiQueryService = aiQueryService;
        }

        [HttpPost("consultar")]
        public async Task<IActionResult> Consultar([FromBody] NaturalLanguageQueryRequest request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Query))
            {
                return BadRequest("Debes enviar el campo 'query' con la petición en lenguaje natural.");
            }

            var result = await _aiQueryService.ResolveAsync(request.Query, cancellationToken);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            // Solo devolvemos la respuesta en lenguaje natural para evitar exponer los datos crudos
            return Ok(new
            {
                response = result.NaturalResponse ?? "No se pudo generar la respuesta en lenguaje natural."
            });
        }
    }
}
