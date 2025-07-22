using System;

namespace GestionVentasServicios.DTO.Cliente
{
    public class UpdateClienteDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public int? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
    }
} 