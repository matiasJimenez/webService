using System.Collections.Generic;
using GestionVentasServicios.DTO.PlanVenta;

namespace GestionVentasServicios.Services
{
    public interface IPlanVentaService
    {
        IEnumerable<PlanVentaDTO> GetAll();
        PlanVentaDTO GetById(int id);
        PlanVentaDTO Create(CreatePlanVentaDTO planVentaDto);
        PlanVentaDTO Update(UpdatePlanVentaDTO planVentaDto);
        void Delete(int id);
    }
} 