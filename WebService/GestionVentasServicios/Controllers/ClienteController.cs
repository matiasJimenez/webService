using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GestionVentasServicios.Data;
using GestionVentasServicios.DTO.Cliente;
using GestionVentasServicios.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GestionVentasServicios.Controllers
{
    [Route("api/[controller]")]
    public class ClienteController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ClienteController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetClientes()
        {
            var clientes = _context.Clientes.ToList().Select(c => c.ToClienteDTO());
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public IActionResult GetCliente([FromRoute] int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente.ToClienteDTO());
        }

        [HttpPost]
        public IActionResult CreateCliente([FromBody] CreateClienteDTO clienteDto)
        {
            if (clienteDto == null)
            {
                return BadRequest("Cliente data is null.");
            }
            var clienteModel = clienteDto.ToClienteFromCreateDTO();
            _context.Clientes.Add(clienteModel);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCliente), new { id = clienteModel.Id }, clienteModel.ToClienteDTO());
        }
    }
}