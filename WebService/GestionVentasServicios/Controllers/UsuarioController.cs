using Microsoft.AspNetCore.Mvc;
using GestionVentasServicios.DTO.Usuario;
using GestionVentasServicios.Services;

namespace GestionVentasServicios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUsuarioDTO dto)
        {
            if (dto == null)
                return BadRequest("Datos inválidos");
            var created = _usuarioService.Create(dto);
            return CreatedAtAction(nameof(Create), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateUsuarioDTO dto)
        {
            if (dto == null || dto.Id != id)
                return BadRequest("Datos inválidos o el Id no coincide.");
            try
            {
                var updated = _usuarioService.Update(dto);
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
                _usuarioService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
} 