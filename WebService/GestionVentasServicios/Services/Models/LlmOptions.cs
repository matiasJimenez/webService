namespace GestionVentasServicios.Services.Models
{
    public class LlmOptions
    {
        public string Provider { get; set; } = "openai"; // openai | ollama
        public string ApiKey { get; set; } = string.Empty; // opcional en ollama
        public string Model { get; set; } = "gpt-4o-mini";
        public string BaseUrl { get; set; } = "https://api.openai.com/v1"; // para ollama: http://localhost:11434
    }
}
