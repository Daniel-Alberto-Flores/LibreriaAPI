using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models.ViewModels
{
    public class LibroViewModel
    {
        [Required(ErrorMessage = "Campo obligatorio.")]
        [Range(1, double.MaxValue, ErrorMessage = "El valor del Id debe ser mayor a 0.")]
        public int IdLibro { get; set; }

        [Required(ErrorMessage = "Campo obligatorio.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "Campo obligatorio.")]
        public short Publicacion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio.")]
        public int AutorId { get; set; }
    }
}
