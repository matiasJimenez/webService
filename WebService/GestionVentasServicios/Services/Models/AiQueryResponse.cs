using System.Collections.Generic;

namespace GestionVentasServicios.Services.Models
{
    public class AiQueryResponse
    {
        public AiQueryPlan? Plan { get; set; }
        public object? Data { get; set; }
        public string? NaturalResponse { get; set; }
        public string? Error { get; set; }
        public List<string> Notes { get; set; } = new();

        public bool Success => string.IsNullOrWhiteSpace(Error);
    }
}
