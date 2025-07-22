using System;

namespace GestionVentasServicios.DTO.PlanVenta
{
    public class CreatePlanVentaDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int CantidadCuotas { get; set; }
    }
} 