using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionVentasServicios.DTO.Cliente;
using GestionVentasServicios.Models;

namespace GestionVentasServicios.Mappers
{
    public static class ClienteMappers
    {
        // Mappers para convertir entre Cliente y ClienteDTO
        public static ClienteDTO ToClienteDTO(this Cliente cliente)
        {
            return new ClienteDTO
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Email = cliente.Email,
                FechaAlta = cliente.FechaAlta,
                TipoDocumento = cliente.TipoDocumento,
                NumeroDocumento = cliente.NumeroDocumento,
                Telefono = cliente.Telefono,
                Direccion = cliente.Direccion
            };
        }
        
        // Mappers para convertir entre ClienteCreateDTO y Cliente
        public static Cliente ToClienteFromCreateDTO(this CreateClienteDTO clienteDto)
        {
            return new Cliente
            {
                Nombre = clienteDto.Nombre,
                Apellido = clienteDto.Apellido,
                Email = clienteDto.Email,
                TipoDocumento = clienteDto.TipoDocumento,
                NumeroDocumento = clienteDto.NumeroDocumento,
                Telefono = clienteDto.Telefono,
                Direccion = clienteDto.Direccion
            };
        }
    }
}