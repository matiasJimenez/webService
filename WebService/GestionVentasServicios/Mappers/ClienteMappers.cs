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
        // Mappers para convertir de Cliente a ClienteDTO
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

        // Mappers para convertir de ClienteCreateDTO a Cliente
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
        
        // Mapper para convertir entre ClienteUpdateDTO y Cliente
        public static void UpdateClienteFromDTO(this Cliente cliente, UpdateClienteDTO clienteDto)
        {
            if (!string.IsNullOrEmpty(clienteDto.Nombre))
                cliente.Nombre = clienteDto.Nombre;
            if (!string.IsNullOrEmpty(clienteDto.Apellido))
                cliente.Apellido = clienteDto.Apellido;
            if (!string.IsNullOrEmpty(clienteDto.Email))
                cliente.Email = clienteDto.Email;
            if (clienteDto.TipoDocumento.HasValue)
                cliente.TipoDocumento = clienteDto.TipoDocumento.Value;
            if (!string.IsNullOrEmpty(clienteDto.NumeroDocumento))
                cliente.NumeroDocumento = clienteDto.NumeroDocumento;
            if (!string.IsNullOrEmpty(clienteDto.Telefono))
                cliente.Telefono = clienteDto.Telefono;
            if (!string.IsNullOrEmpty(clienteDto.Direccion))
                cliente.Direccion = clienteDto.Direccion;
        }

        // Mapper para convertir Cliente a UpdateClienteDTO (opcional)
        public static UpdateClienteDTO ToUpdateClienteDTO(this Cliente cliente)
        {
            return new UpdateClienteDTO
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Email = cliente.Email,
                TipoDocumento = cliente.TipoDocumento,
                NumeroDocumento = cliente.NumeroDocumento,
                Telefono = cliente.Telefono,
                Direccion = cliente.Direccion
            };
        }

        // Mapper para convertir una lista de Cliente a una lista de ClienteDTO
        public static List<ClienteDTO> ToClienteDTOList(this IEnumerable<Cliente> clientes)
        {
            return clientes.Select(c => c.ToClienteDTO()).ToList();
        }
    }
}