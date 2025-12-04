using System.ComponentModel.DataAnnotations;

namespace Sistemas.Models
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }

        [Required, StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Apellido { get; set; }

        [StringLength(15)]
        public string Telefono { get; set; }

        [StringLength(100), EmailAddress]
        public string Email { get; set; }

        [StringLength(100)]
        public string Direccion { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Propiedades de clasificación
        [StringLength(30)]
        public string? Clasificacion { get; set; } // VIP, Frecuente, Normal, Nuevo
    }
}