using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistemas.Models
{
    public class Desempeno
    {
        [Key]
        public int IdDesempeno { get; set; }

        [Required]
        public int IdEmpleado { get; set; }
        [ForeignKey("IdEmpleado")]
        public Empleado? Empleado { get; set; }

        [Required]
        public DateTime FechaEvaluacion { get; set; } = DateTime.UtcNow;

        [Range(1, 5)]
        public int Calificacion { get; set; }

        public int ServiciosCompletados { get; set; }

        public decimal TiempoPromedioServicio { get; set; } // En minutos

        [StringLength(500)]
        public string? Comentarios { get; set; }

        public DateTime PeriodoInicio { get; set; }

        public DateTime PeriodoFin { get; set; }
    }
}