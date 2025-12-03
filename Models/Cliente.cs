// Models/Cliente.cs
// Sprint 3 - Control de Clientes (HU0301 Registrar Cliente, HU0302 Editar, HU0303 Eliminar, HU0304 Buscar, HU0305 Historial)
using System.ComponentModel.DataAnnotations;

namespace Sistemas.Models
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; } // id_cliente
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
    }
}
