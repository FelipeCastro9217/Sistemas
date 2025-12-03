using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistemas.Models
{
    public class Asistencia
    {
        [Key]
        public int IdAsistencia { get; set; }

        [Required]
        public int IdEmpleado { get; set; }
        [ForeignKey("IdEmpleado")]
        public Empleado? Empleado { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        public DateTime? HoraEntrada { get; set; }

        public DateTime? HoraSalida { get; set; }

        [StringLength(20)]
        public string Estado { get; set; } = "Presente"; // Presente, Ausente, Retardo, Justificado

        [StringLength(200)]
        public string? Observaciones { get; set; }
    }
}