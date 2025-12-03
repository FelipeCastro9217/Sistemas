using System.ComponentModel.DataAnnotations;

namespace Sistemas.Models
{
    public class AuditoriaRegistro
    {
        [Key]
        public int IdAuditoria { get; set; }

        [Required, StringLength(50)]
        public string TipoEntidad { get; set; } // Empleado, Tarea, Asistencia, etc.

        [Required]
        public int IdEntidad { get; set; }

        [Required, StringLength(50)]
        public string Accion { get; set; } // Crear, Editar, Eliminar

        [Required]
        public DateTime FechaHora { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? UsuarioResponsable { get; set; }

        [StringLength(500)]
        public string? DatosAnteriores { get; set; }

        [StringLength(500)]
        public string? DatosNuevos { get; set; }
    }
}