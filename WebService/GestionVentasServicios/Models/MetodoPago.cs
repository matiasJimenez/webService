using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionVentasServicios.Models
{
    public class MetodoPago
    {
        public int Id { get; set; } // Identificador único del método de pago
        public string Tipo { get; set; } = string.Empty; // Tipo de método de pago (ej. "Efectivo", "Tarjeta de crédito", "Transferencia", etc.)
        public string Descripcion { get; set; } = string.Empty; // Descripción del método de pago
        public DateTime FechaAlta { get; set; } = DateTime.Now; // Fecha de alta del método de pago
        public bool Estado { get; set; } // Puede ser "Activo" o "Inactivo"
        public int IdUsuarioAlta { get; set; } // Usuario que dio de alta el método de pago
        public int IdUsuarioModificacion { get; set; } // Usuario que modificó el método de pago
        public DateTime FechaModificacion { get; set; } // Fecha de modificación
        public string Observaciones { get; set; } = string.Empty; // Observaciones sobre el método de pago
        public string TipoComision { get; set; } = string.Empty; // Puede ser "Fija" o "Variable"
    }
}