using System;
using GestionVentasServicios.Data;
using GestionVentasServicios.DTO.Usuario;
using GestionVentasServicios.Models;
using GestionVentasServicios.Mappers;

namespace GestionVentasServicios.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDBContext _context;

        public UsuarioService(ApplicationDBContext context)
        {
            _context = context;
        }

        public UsuarioDTO Create(CreateUsuarioDTO usuarioDto)
        {
            var usuario = usuarioDto.ToUsuarioFromCreateDTO();
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return usuario.ToUsuarioDTO();
        }

        public UsuarioDTO Update(UpdateUsuarioDTO usuarioDto)
        {
            var usuario = _context.Usuarios.Find(usuarioDto.Id);
            if (usuario == null)
                throw new Exception($"Usuario con Id {usuarioDto.Id} no encontrado");
            usuario.UpdateUsuarioFromDTO(usuarioDto);
            _context.SaveChanges();
            return usuario.ToUsuarioDTO();
        }

        public void Delete(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
                throw new Exception($"Usuario con Id {id} no encontrado");
            usuario.Estado = false;
            _context.SaveChanges();
        }
    }
} 