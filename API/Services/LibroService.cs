using API.Models;
using API.Models.ViewModels;
using API.Tools.Validators;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using FluentValidation.Results;

namespace API.Services
{
    public class LibroService : ILibroService
    {
        private readonly LibreriaDbContext _db;

        public LibroService(LibreriaDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Libro> GetLibros()
        {
            List<Libro> listLibros = new();
            try
            {
                var lst = _db.Libros.Include(a => a.Autor).OrderBy(m => m.Nombre).ToList();
                listLibros = lst;
            }
            catch
            {
                throw new Exception(Tools.MessageFilter.ObtenerMensaje(1));
            }
            return listLibros;
        }

        public Libro GetLibro(int _id)
        {
            Libro oLibro = new();
            try
            {
                var result = _db.Libros.Find(_id);

                if (result != null)
                    oLibro = result;
            }
            catch
            {
                throw new Exception(Tools.MessageFilter.ObtenerMensaje(2));
            }            
            return oLibro;
        }

        public Libro GetLibroPorNombreAutor(string _nombre, string _autor)
        {
            Libro oLibro = new();
            try
            {
                var result = _db.Libros.Where(l => l.Nombre == _nombre
                                                && (l.Autor.Nombre + l.Autor.Apellido) == _autor)
                                              .FirstOrDefault();

                if (result != null)
                    oLibro = result;
            }
            catch
            {
                throw new Exception(Tools.MessageFilter.ObtenerMensaje(2));
            }
            return oLibro;
        }

        public void Add(LibroViewModel _model)
        {
            var messageValidation = string.Empty;
            try
            {
                LibroValidator validator = new LibroValidator(_db);
                ValidationResult results = validator.Validate(_model);

                if (!results.IsValid)
                {
                    // Separamos cada mensaje con '. '
                    messageValidation = results.ToString(". ");
                    throw new Exception(messageValidation);
                }
                else
                {
                    Libro oLibro = new()
                    {
                        Nombre = _model.Nombre,
                        Publicacion = _model.Publicacion,
                        AutorId = _model.AutorId
                    };
                    using (var scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        _db.Libros.Add(oLibro);
                        _db.SaveChanges();
                        scope.Complete();
                    }
                }
            }
            catch(Exception ex)
            {
                if (messageValidation != string.Empty)
                    throw new Exception(ex.Message);
                else
                    throw new Exception(Tools.MessageFilter.ObtenerMensaje(4));// 4 => es una excepción                           
            }
        }

        public void Edit(LibroViewModel _model)
        {
            var messageValidation = string.Empty;
            try
            {
                LibroValidator validator = new LibroValidator(_db);
                ValidationResult results = validator.Validate(_model);

                if (!results.IsValid)
                {
                    messageValidation = results.ToString(". ");
                    throw new Exception(messageValidation);
                }
                else
                {
                    using (var transaction = _db.Database.BeginTransaction())
                    {
                        Libro oLibro = _db.Libros.Find(_model.IdLibro)!;
                        oLibro.Nombre = _model.Nombre;
                        oLibro.Publicacion = _model.Publicacion;
                        oLibro.AutorId = _model.AutorId;
                        _db.Entry(oLibro).State = EntityState.Modified;
                        _db.SaveChanges();
                        transaction.Commit();
                    }
                }                
            }
            catch(Exception ex)
            {
                if (messageValidation != string.Empty)
                    throw new Exception(ex.Message);
                else
                    throw new Exception(Tools.MessageFilter.ObtenerMensaje(6));// 6 => es una excepción     
            }
        }
    }
}
