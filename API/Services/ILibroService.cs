using API.Models;
using API.Models.ViewModels;

namespace API.Services
{
    public interface ILibroService
    {
        public IEnumerable<Libro> GetLibros();
        public Libro GetLibro(int _id);
        public Libro GetLibroPorNombreAutor(string _nombre, string _autor);
        public void Add(LibroViewModel _model);
        public void Edit(LibroViewModel _model);
    }
}
