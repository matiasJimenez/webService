using System;

namespace GestionVentasServicios.DTO.PlanVenta
{
    public class PlanVentaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int CantidadCuotas { get; set; }
        public DateTime FechaAlta { get; set; }
    }
} 