// Models/Pago.cs
// Archivo opcional: pagos (puede usarse con servicios/turnos)
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistemas.Models
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }
        public int IdServicio { get; set; }
        [ForeignKey("IdServicio")]
        public Servicio Servicio { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; } = DateTime.UtcNow;
        [StringLength(50)]
        public string Metodo { get; set; } // Efectivo, Tarjeta, etc.
    }
}
