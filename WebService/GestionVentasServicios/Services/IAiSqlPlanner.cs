using System.Threading;
using System.Threading.Tasks;
using GestionVentasServicios.Services.Models;

namespace GestionVentasServicios.Services
{
    public interface IAiSqlPlanner
    {
        Task<AiQueryPlan?> BuildPlanAsync(string naturalLanguageQuery, CancellationToken cancellationToken = default);
    }
}
