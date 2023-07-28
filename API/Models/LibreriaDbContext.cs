using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public partial class LibreriaDbContext : DbContext
    {
        public LibreriaDbContext(DbContextOptions<LibreriaDbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Libro> Libros { get; set; }
        public virtual DbSet<Autor> Autores { get; set; }
        public virtual DbSet<Upload> Uploads { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;DataBase=LibreriaDB; Trusted_Connection=True; TrustServerCertificate=true; ConnectRetryCount=0");
    }
}
