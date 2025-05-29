using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionVentasServicios.DTO.Cliente;

namespace GestionVentasServicios.Services
{
    public interface IClienteService
    {
        IEnumerable<ClienteDTO> GetAll();
        ClienteDTO GetById(int id);
        ClienteDTO Create(CreateClienteDTO clienteDto);
    }
}