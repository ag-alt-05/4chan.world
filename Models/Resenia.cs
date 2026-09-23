using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusCima.Models
{
    [Table("Resenias")]
    public class Resenia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Usuario { get; set; } = "Anonimo";

        [Required]
        [StringLength(200)]
        public string Mensaje { get; set; }

        [Required]
        [Range(1, 5)]
        public int Estrellas { get; set; }
        
        [Required]
        [StringLength(100)]
        public string RutaNombre { get; set; }
    }
}