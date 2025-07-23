using System.Collections.Generic;
using GestionVentasServicios.DTO.Usuario;

namespace GestionVentasServicios.Services
{
    public interface IUsuarioService
    {
        UsuarioDTO Create(CreateUsuarioDTO usuarioDto);
        UsuarioDTO Update(UpdateUsuarioDTO usuarioDto);
        void Delete(int id); // Cambia Estado a false
    }
} 