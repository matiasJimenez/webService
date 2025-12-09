using System.Threading;
using System.Threading.Tasks;

namespace GestionVentasServicios.Services
{
    public interface IAiAnswerFormatter
    {
        Task<string?> BuildNaturalResponseAsync(string userQuery, string jsonData, CancellationToken cancellationToken = default);
    }
}
