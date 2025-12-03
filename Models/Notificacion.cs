// Models/Notificacion.cs
// Sprint 3 - Enviar aviso al cliente (HU0304)
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistemas.Models
{
    public class Notificacion
    {
        [Key]
        public int IdNotificacion { get; set; }
        public int IdCliente { get; set; }
        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }
        [StringLength(300)]
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        [StringLength(30)]
        public string Estado { get; set; } // Entregado, Pendiente, Fallido
    }
}
