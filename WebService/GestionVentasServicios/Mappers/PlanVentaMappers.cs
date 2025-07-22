using System.Collections.Generic;
using System.Linq;
using GestionVentasServicios.DTO.PlanVenta;
using GestionVentasServicios.Models;

namespace GestionVentasServicios.Mappers
{
    public static class PlanVentaMappers
    {
        public static PlanVentaDTO ToPlanVentaDTO(this PlanVenta plan)
        {
            return new PlanVentaDTO
            {
                Id = plan.Id,
                Nombre = plan.Nombre,
                Descripcion = plan.Descripcion,
                CantidadCuotas = plan.CantidadCuotas,
                FechaAlta = plan.FechaAlta
            };
        }

        public static PlanVenta ToPlanVentaFromCreateDTO(this CreatePlanVentaDTO dto)
        {
            return new PlanVenta
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                CantidadCuotas = dto.CantidadCuotas,
                FechaAlta = System.DateTime.Now
            };
        }

        public static void UpdatePlanVentaFromDTO(this PlanVenta plan, UpdatePlanVentaDTO dto)
        {
            if (!string.IsNullOrEmpty(dto.Nombre))
                plan.Nombre = dto.Nombre;
            if (!string.IsNullOrEmpty(dto.Descripcion))
                plan.Descripcion = dto.Descripcion;
            if (dto.CantidadCuotas.HasValue)
                plan.CantidadCuotas = dto.CantidadCuotas.Value;
        }

        public static List<PlanVentaDTO> ToPlanVentaDTOList(this IEnumerable<PlanVenta> planes)
        {
            return planes.Select(p => p.ToPlanVentaDTO()).ToList();
        }
    }
} 