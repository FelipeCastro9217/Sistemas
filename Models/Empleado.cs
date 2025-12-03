using System.ComponentModel.DataAnnotations;

namespace Sistemas.Models
{
    public class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }

        [Required, StringLength(50)]
        public string Nombre { get; set; }

        [Required, StringLength(50)]
        public string Apellido { get; set; }

        [Required, StringLength(50)]
        public string Puesto { get; set; }

        [Required, StringLength(10)]
        public string Telefono { get; set; }

        [Required, StringLength(20)]
        public string? Disponibilidad { get; set; }
    }
}
