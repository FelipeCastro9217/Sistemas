using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistemas.Models
{
    public class Tarea
    {
        [Key]
        public int IdTarea { get; set; }

        [Required]
        public int IdEmpleado { get; set; }
        [ForeignKey("IdEmpleado")]
        public Empleado? Empleado { get; set; }

        [Required, StringLength(100)]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required, StringLength(20)]
        public string Estado { get; set; } = "Pendiente"; // Pendiente, En Proceso, Completada, Cancelada

        [StringLength(200)]
        public string? Observaciones { get; set; }

        [Required, StringLength(30)]
        public string Prioridad { get; set; } = "Media"; // Alta, Media, Baja
    }
}