using System;

namespace GestionVentasServicios.DTO.PlanVenta
{
    public class UpdatePlanVentaDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int? CantidadCuotas { get; set; }
    }
} 