using API.Models;

namespace API.Services
{
    public interface IArchivoService
    {
        public byte[] GeneraListadoLibros(List<Libro> l_listLibros);
    }
}
