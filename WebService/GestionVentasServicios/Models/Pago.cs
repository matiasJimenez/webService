using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GestionVentasServicios.Models
{
    public class Pago
    {
        public int Id { get; set; } // ID del pago
        public int ClientId { get; set; } // ID del cliente que realizó el pago
        public DateTime FechaPago { get; set; } // Fecha en la que se realizó el pago
        public DateTime FechaAlta { get; set; } = DateTime.Now; // Fecha de alta del pago
        [Column("decimal(18,2)")]
        [Precision(18, 2)] // Precisión para el monto del pago
        public decimal Monto { get; set; }  // Monto del pago
        public int NumeroCuota { get; set; } // Cantidad de cuotas pagadas
        public int PlanVentaId { get; set; } // ID del plan de venta asociado
        public int UsuarioId { get; set; } // ID del usuario que realizó el pago
        public int MetodoPagoId { get; set; } // ID del método de pago utilizado
        public string Observaciones { get; set; } = string.Empty; // Puede ser "Pago en efectivo", "Pago con tarjeta", etc.
        public string Estado { get; set; } = string.Empty; // Puede ser "Pendiente", "Pagado", "Anulado"
    }
}