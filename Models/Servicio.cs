// Models/Servicio.cs
// Sprint 2 - Servicio realizado (registro de resultados, observaciones)
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistemas.Models
{
    public class Servicio
    {
        [Key]
        public int IdServicio { get; set; } // id_servicio
        public DateTime Fecha { get; set; } = DateTime.UtcNow; // fecha
        // FK
        public int IdCliente { get; set; }
        [ForeignKey("IdCliente")]
        public Cliente? Cliente { get; set; }

        public int? IdEmpleado { get; set; }
        [ForeignKey("IdEmpleado")]
        public Empleado? Empleado { get; set; }

        public int IdTipoServicio { get; set; }
        [ForeignKey("IdTipoServicio")]
        public TipoServicio? TipoServicio { get; set; }

        [StringLength(200)]
        public string? ResultadoServicio { get; set; }

        [StringLength(200)]
        public string? Observaciones { get; set; }
    }
}
