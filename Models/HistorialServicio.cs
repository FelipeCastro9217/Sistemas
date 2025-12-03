// Models/HistorialServicio.cs
// Sprint 3 - Historial de servicios para cliente
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistemas.Models
{
    public class HistorialServicio
    {
        [Key]
        public int IdHistorial { get; set; }
        public int IdCliente { get; set; }
        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }
        public int IdVehiculo { get; set; }
        [ForeignKey("IdVehiculo")]
        public Vehiculo Vehiculo { get; set; }
        public DateTime FechaServicio { get; set; }
        [StringLength(50)]
        public string TipoServicio { get; set; }
        [StringLength(200)]
        public string ResultadoServicio { get; set; }
        [StringLength(200)]
        public string Observaciones { get; set; }
    }
}
