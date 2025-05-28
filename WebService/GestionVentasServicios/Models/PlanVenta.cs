using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionVentasServicios.Models
{
    public class PlanVenta
    {
        public int Id { get; set; } // Identificador único del plan de venta
        public string Nombre { get; set; } = string.Empty; // Nombre del plan de venta
        public string Descripcion { get; set; } = string.Empty; // Descripción del plan de venta
        public int CantidadCuotas { get; set; } // Cantidad de cuotas del plan de venta
        public DateTime FechaAlta { get; set; } = DateTime.Now; // Fecha de alta del plan de venta
    }
}