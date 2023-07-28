using API.GeneraArchivos;
using API.Models;

namespace API.Services
{
    public class ArchivoService : IArchivoService
    {
        private readonly LibreriaDbContext _db;

        public ArchivoService(LibreriaDbContext db)
        {
            _db = db;
        }

        public byte[] GeneraListadoLibros(List<Libro> _listLibros)
        {
            try
            {
                return ListadoLibros.GeneraArchivos(_listLibros);
            }
            catch
            {
                throw new Exception(Tools.MessageFilter.ObtenerMensaje(10));
            }
            
        }
    }
}
