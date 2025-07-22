using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionVentasServicios.Data;
using GestionVentasServicios.DTO.Cliente;
using GestionVentasServicios.Mappers;

namespace GestionVentasServicios.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDBContext _context;

        public ClienteService(ApplicationDBContext context)
        {
            _context = context;
        }

        public IEnumerable<ClienteDTO> GetAll()
        {
            return _context.Clientes.ToClienteDTOList();
        }

        public ClienteDTO GetById(int id)
        {
            var cliente = _context.Clientes.Find(id);
            return cliente != null ? cliente.ToClienteDTO() : new ClienteDTO();
        }

        public ClienteDTO Create(CreateClienteDTO clienteDto)
        {
            var cliente = clienteDto.ToClienteFromCreateDTO();
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
            return cliente.ToClienteDTO();
        }

        public ClienteDTO Update(UpdateClienteDTO clienteDto)
        {
            var cliente = _context.Clientes.Find(clienteDto.Id);
            if (cliente == null)
            {
                throw new Exception($"Cliente con Id {clienteDto.Id} no encontrado");
            }
            cliente.UpdateClienteFromDTO(clienteDto);
            _context.SaveChanges();
            return cliente.ToClienteDTO();
        }

        public void Delete(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
                throw new Exception($"Cliente con Id {id} no encontrado");
            _context.Clientes.Remove(cliente);
            _context.SaveChanges();
        }
    }
}