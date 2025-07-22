using System;
using System.Collections.Generic;
using System.Linq;
using GestionVentasServicios.Data;
using GestionVentasServicios.DTO.PlanVenta;
using GestionVentasServicios.Mappers;

namespace GestionVentasServicios.Services
{
    public class PlanVentaService : IPlanVentaService
    {
        private readonly ApplicationDBContext _context;

        public PlanVentaService(ApplicationDBContext context)
        {
            _context = context;
        }

        public IEnumerable<PlanVentaDTO> GetAll()
        {
            return _context.PlanVentas.ToPlanVentaDTOList();
        }

        public PlanVentaDTO GetById(int id)
        {
            var plan = _context.PlanVentas.Find(id);
            return plan != null ? plan.ToPlanVentaDTO() : new PlanVentaDTO();
        }

        public PlanVentaDTO Create(CreatePlanVentaDTO planVentaDto)
        {
            var plan = planVentaDto.ToPlanVentaFromCreateDTO();
            _context.PlanVentas.Add(plan);
            _context.SaveChanges();
            return plan.ToPlanVentaDTO();
        }

        public PlanVentaDTO Update(UpdatePlanVentaDTO planVentaDto)
        {
            var plan = _context.PlanVentas.Find(planVentaDto.Id);
            if (plan == null)
                throw new Exception($"PlanVenta con Id {planVentaDto.Id} no encontrado");
            plan.UpdatePlanVentaFromDTO(planVentaDto);
            _context.SaveChanges();
            return plan.ToPlanVentaDTO();
        }

        public void Delete(int id)
        {
            var plan = _context.PlanVentas.Find(id);
            if (plan == null)
                throw new Exception($"PlanVenta con Id {id} no encontrado");
            _context.PlanVentas.Remove(plan);
            _context.SaveChanges();
        }
    }
} 