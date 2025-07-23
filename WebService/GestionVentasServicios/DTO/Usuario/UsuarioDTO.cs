using System;

namespace GestionVentasServicios.DTO.Usuario
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime FechaAlta { get; set; }
        public string Rol { get; set; } = string.Empty;
        public DateTime FechaUltimoAcceso { get; set; }
        public bool Estado { get; set; }
    }
} 