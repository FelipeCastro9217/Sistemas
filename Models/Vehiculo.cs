// Models/Vehiculo.cs
// Sprint 3 - Asociar vehículo al cliente
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistemas.Models
{
    public class Vehiculo
    {
        [Key]
        public int IdVehiculo { get; set; } // id_vehiculo
        [Required, StringLength(10)]
        public string Placa { get; set; }
        [StringLength(50)]
        public string Marca { get; set; }
        [StringLength(50)]
        public string Modelo { get; set; }
        [StringLength(30)]
        public string Color { get; set; }
        [StringLength(30)]
        public string Tipo { get; set; } // carro, moto, camioneta
        // FK
        public int IdCliente { get; set; }
        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }
    }
}
