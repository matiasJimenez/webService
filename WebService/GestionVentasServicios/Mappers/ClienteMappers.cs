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
        
        // Mapper para convertir entre ClienteUpdateDTO y Cliente
        /*public static void UpdateClienteFromDTO(this Cliente cliente, UpdateClienteDTO clienteDto)
        {
            cliente.Nombre = clienteDto.Nombre;
            cliente.Apellido = clienteDto.Apellido;
            cliente.Email = clienteDto.Email;
            cliente.TipoDocumento = clienteDto.TipoDocumento;
            cliente.NumeroDocumento = clienteDto.NumeroDocumento;
            cliente.Telefono = clienteDto.Telefono;
            cliente.Direccion = clienteDto.Direccion;
        }

        // Mapper para convertir Cliente a UpdateClienteDTO (opcional)
        public static UpdateClienteDTO ToUpdateClienteDTO(this Cliente cliente)
        {
            return new UpdateClienteDTO
            {
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Email = cliente.Email,
                TipoDocumento = cliente.TipoDocumento,
                NumeroDocumento = cliente.NumeroDocumento,
                Telefono = cliente.Telefono,
                Direccion = cliente.Direccion
            };
        }*/

        // Mapper para convertir una lista de Cliente a una lista de ClienteDTO
        public static List<ClienteDTO> ToClienteDTOList(this IEnumerable<Cliente> clientes)
        {
            return clientes.Select(c => c.ToClienteDTO()).ToList();
        }
    }
}