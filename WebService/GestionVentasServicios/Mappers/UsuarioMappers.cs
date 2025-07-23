using System.Collections.Generic;
using System.Linq;
using GestionVentasServicios.DTO.Usuario;
using GestionVentasServicios.Models;

namespace GestionVentasServicios.Mappers
{
    public static class UsuarioMappers
    {
        public static UsuarioDTO ToUsuarioDTO(this Usuario usuario)
        {
            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                FechaAlta = usuario.FechaAlta,
                Rol = usuario.Rol,
                FechaUltimoAcceso = usuario.FechaUltimoAcceso,
                Estado = usuario.Estado
            };
        }

        public static Usuario ToUsuarioFromCreateDTO(this CreateUsuarioDTO dto)
        {
            return new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Contraseña = dto.Contraseña,
                Rol = dto.Rol,
                FechaAlta = System.DateTime.Now,
                FechaUltimoAcceso = System.DateTime.Now,
                Estado = true
            };
        }

        public static void UpdateUsuarioFromDTO(this Usuario usuario, UpdateUsuarioDTO dto)
        {
            if (!string.IsNullOrEmpty(dto.Nombre))
                usuario.Nombre = dto.Nombre;
            if (!string.IsNullOrEmpty(dto.Apellido))
                usuario.Apellido = dto.Apellido;
            if (!string.IsNullOrEmpty(dto.Email))
                usuario.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Contraseña))
                usuario.Contraseña = dto.Contraseña;
            if (!string.IsNullOrEmpty(dto.Rol))
                usuario.Rol = dto.Rol;
            if (dto.Estado.HasValue)
                usuario.Estado = dto.Estado.Value;
        }

        public static List<UsuarioDTO> ToUsuarioDTOList(this IEnumerable<Usuario> usuarios)
        {
            return usuarios.Select(u => u.ToUsuarioDTO()).ToList();
        }
    }
} 