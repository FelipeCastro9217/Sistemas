using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistemas.Models
{
    public class ClasificacionCliente
    {
        [Key]
        public int IdClasificacion { get; set; }

        [Required]
        public int IdCliente { get; set; }
        [ForeignKey("IdCliente")]
        public Cliente? Cliente { get; set; }

        [Required, StringLength(30)]
        public string TipoClasificacion { get; set; } // VIP, Frecuente, Normal, Nuevo

        [StringLength(200)]
        public string? Observaciones { get; set; }

        public DateTime FechaClasificacion { get; set; } = DateTime.UtcNow;

        public int ServiciosRealizados { get; set; }

        public decimal TotalGastado { get; set; }
    }
}