using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistemas.Models
{
    public class Turno
    {
        [Key]
        public int Ctur { get; set; }  // ID Turno (PK)

        [Required]
        public int Ccli { get; set; }  // Cliente
        [ForeignKey("Ccli")]

        public Cliente Cliente { get; set; }

        public int? Cemp { get; set; }  // Empleado
        [ForeignKey("Cemp")]
        public Empleado Empleado { get; set; }

        public int? Cbah { get; set; }  // Bahía

        [DataType(DataType.Date)]
        public DateTime Ftrn { get; set; }  // Fecha

        [StringLength(5)]
        public string Htrn { get; set; }  // Hora HH:MM

        [StringLength(20)]
        public string Estr { get; set; }  // Pendiente / En Proceso / Finalizado / Cancelado
    }
}
