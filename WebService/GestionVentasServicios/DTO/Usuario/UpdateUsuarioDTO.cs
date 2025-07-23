using System;

namespace GestionVentasServicios.DTO.Usuario
{
    public class UpdateUsuarioDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? Contraseña { get; set; }
        public string? Rol { get; set; }
        public bool? Estado { get; set; }
    }
} 