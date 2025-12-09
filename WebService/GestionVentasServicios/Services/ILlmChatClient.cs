using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GestionVentasServicios.Services
{
    public interface ILlmChatClient
    {
        Task<string?> SendAsync(IEnumerable<(string role, string content)> messages, bool jsonResponse, CancellationToken cancellationToken = default);
    }
}
