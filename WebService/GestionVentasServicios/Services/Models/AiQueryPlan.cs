using System.Collections.Generic;

namespace GestionVentasServicios.Services.Models
{
    public class AiQueryPlan
    {
        public string Entity { get; set; } = string.Empty;
        public List<string> Select { get; set; } = new();
        public List<AiQueryFilter> Filters { get; set; } = new();
        public int? Limit { get; set; }
    }

    public class AiQueryFilter
    {
        public string Field { get; set; } = string.Empty;
        public string? Entity { get; set; }
        public string Operator { get; set; } = "equals";
        public string? Value { get; set; }
    }
}
