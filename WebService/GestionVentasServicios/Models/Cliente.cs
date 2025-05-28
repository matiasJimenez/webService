using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionVentasServicios.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty; // Nombre de la persona
        public string Apellido { get; set; } = string.Empty; // Apellido de la persona
        public string Email { get; set; } = string.Empty; // Email de la persona
        public DateTime FechaAlta { get; set; } = DateTime.Now; // Fecha de alta de la persona
        public int TipoDocumento { get; set; } // 1: DNI, 2: Pasaporte, 3: Cédula de identidad
        public string NumeroDocumento { get; set; } = string.Empty; // Número de documento
        public string Telefono { get; set; } = string.Empty; // Teléfono
        public string Direccion { get; set; } = string.Empty; // Dirección
    }
}