using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Libro
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLibro { get; set; }
        [Required]
        [StringLength(100)]
        public string? Nombre { get; set; }
        [Required]
        public short Publicacion { get; set; }
        [Required]
        public int AutorId { get; set; }

        [ForeignKey(nameof(AutorId))]
        public virtual Autor Autor { get; set; }
    }
}
