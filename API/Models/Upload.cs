using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Upload
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUpload { get; set; }
        [Required]
        [StringLength(100)]
        public string? Descripcion { get; set; }
        [Required]
        public int UsuarioId { get; set; }
        [Required]
        public DateTime FechaDeModificacion { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public virtual Usuario Usuario { get; set; }
    }
}
