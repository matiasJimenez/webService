using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionVentasServicios.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty; // Nombre de la persona
        public string Apellido { get; set; } = string.Empty; // Apellido de la persona
        public string Email { get; set; } = string.Empty; // Email de la persona
        public DateTime FechaAlta { get; set; } = DateTime.Now; // Fecha de alta de la persona
        public string Contraseña { get; set; } = string.Empty; // Encriptada
        public string Rol { get; set; } = string.Empty; // Puede ser "Cliente", "Vendedor", "Administrador"
        public DateTime FechaUltimoAcceso { get; set; } // Fecha del último acceso
        public bool Estado { get; set; } // Puede ser "Activo", "Inactivo", "Suspendido"
    }
}