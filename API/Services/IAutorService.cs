using API.Models;

namespace API.Services
{
    public interface IAutorService
    {
        public IEnumerable<Autor> GetAutores();
        public Autor GetAutorPorId(int _autorId);
    }
}
