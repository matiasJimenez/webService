using Microsoft.AspNetCore.Mvc;
using GestionVentasServicios.DTO.PlanVenta;
using GestionVentasServicios.Services;

namespace GestionVentasServicios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanVentaController : ControllerBase
    {
        private readonly IPlanVentaService _planVentaService;

        public PlanVentaController(IPlanVentaService planVentaService)
        {
            _planVentaService = planVentaService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var planes = _planVentaService.GetAll();
            return Ok(planes);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var plan = _planVentaService.GetById(id);
            if (plan == null)
                return NotFound();
            return Ok(plan);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreatePlanVentaDTO dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos");
            var created = _planVentaService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdatePlanVentaDTO dto)
        {
            if (dto == null || dto.Id != id)
                return BadRequest("Datos inválidos o el Id no coincide.");
            try
            {
                var updated = _planVentaService.Update(dto);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _planVentaService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
} 