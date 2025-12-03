// Models/TipoServicio.cs
// Sprint 2 - Registro de servicios (HU0201..HU0205)
using System.ComponentModel.DataAnnotations;

namespace Sistemas.Models
{
    public class TipoServicio
    {
        [Key]
        public int IdTipoServicio { get; set; } // id_tipo_servicio
        [Required, StringLength(50)]
        public string Nombre { get; set; }
        [StringLength(150)]
        public string Descripcion { get; set; }
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; } // decimal(10,2)
    }
}
