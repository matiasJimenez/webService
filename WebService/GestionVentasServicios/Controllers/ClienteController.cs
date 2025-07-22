using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GestionVentasServicios.Data;
using GestionVentasServicios.DTO.Cliente;
using GestionVentasServicios.Mappers;
using GestionVentasServicios.Services; // Add this line if IClienteService is in the Services namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GestionVentasServicios.Controllers
{
    [Route("api/[controller]")]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public IActionResult GetClientes()
        {
            var clientes = _clienteService.GetAll();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public IActionResult GetCliente([FromRoute] int id)
        {
            var cliente = _clienteService.GetById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpPost]
        public IActionResult CreateCliente([FromBody] CreateClienteDTO clienteDto)
        {
            if (clienteDto == null)
            {
                return BadRequest("Cliente data is null.");
            }
            var createdCliente = _clienteService.Create(clienteDto);
            return CreatedAtAction(nameof(GetCliente), new { id = createdCliente.Id }, createdCliente);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCliente([FromRoute] int id, [FromBody] UpdateClienteDTO clienteDto)
        {
            if (clienteDto == null || clienteDto.Id != id)
            {
                return BadRequest("Datos de cliente inválidos o el Id no coincide.");
            }
            try
            {
                var updatedCliente = _clienteService.Update(clienteDto);
                return Ok(updatedCliente);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCliente([FromRoute] int id)
        {
            try
            {
                _clienteService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}