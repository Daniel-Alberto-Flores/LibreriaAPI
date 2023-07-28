using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class AutorService : IAutorService
    {
        private readonly LibreriaDbContext _db;

        public AutorService(LibreriaDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Autor> GetAutores()
        {
            List<Autor> listAutores = new();
            try
            {
                var lst = _db.Autores.OrderBy(a => a.Apellido).ToList();
                listAutores = lst;
            }
            catch
            {
                throw new Exception("Se ha producido un error al obtener el listado de autores.");
            }
            return listAutores;
        }

        public Autor GetAutorPorId(int _autorId)
        {
            Autor autor = new();
            try
            {
                autor = _db.Autores.Where(a => a.IdAutor == _autorId).FirstOrDefault()!;
            }
            catch
            {
                throw new Exception("Se ha producido un error al obtener el autor ingresado.");
            }
            return autor;
        }
    }
}
