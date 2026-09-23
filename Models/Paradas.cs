using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusCima.Models
{
    [Table("Paradas")]
    public class Parada
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdParada { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [Column(TypeName = "decimal(10,8)")]
        public decimal? Latitud { get; set; }

        [Column(TypeName = "decimal(11,8)")]
        public decimal? Longitud { get; set; }

        [StringLength(50)]
        public string RutaNombre { get; set; }

        public int? Orden { get; set; }

        public bool EsWaypoint { get; set; }
    }
}