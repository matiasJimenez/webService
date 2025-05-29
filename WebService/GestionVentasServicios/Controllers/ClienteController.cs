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
    }
}