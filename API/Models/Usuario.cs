using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(256)]
        public string? Password { get; set; }

        [StringLength(100)]
        public string? Nombre { get; set; }
    }
}
