using API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using API.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace API.Tools.Validators
{
    public class LibroValidator : AbstractValidator<LibroViewModel>
    {
        /// <summary>
        /// Contiene las validaciones para LibroViewModel
        /// </summary>

        public LibroValidator(LibreriaDbContext _db)
        {
            RuleFor(x => x.IdLibro)
                .GreaterThanOrEqualTo(0)
                .WithMessage("El campo 'Id' no puede ser menor a cero (0).")
                .Must(x =>
                {
                    if (x == 0)
                        return true;// Se está añadiendo un libro nuevo
                    else
                    {
                        // Se está editando, verificamos que exista el libro que se intenta editar
                        return _db.Libros.Where(l => l.IdLibro == x).Any();
                    }
                })
                .WithMessage(x => $"No existe el id del libro ingresado. Id del libro: {x.IdLibro}");

            RuleFor(x => x.Nombre)
                .NotEmpty()
                .WithMessage("Debe ingresar el campo 'Nombre' del libro.");

            RuleFor(x => x.Publicacion)
                .NotEmpty()
                .WithMessage("Debe ingresar el campo 'Publicación' del libro.");

            RuleFor(x => x.AutorId)
                .NotEmpty()
                .WithMessage("Debe ingresar el campo 'AutorId' del libro.")
                .Must(x =>
                {
                    return _db.Autores.Where(a => a.IdAutor == x).Any();
                })
                .WithMessage(x => $"No existe el id del autor ingresado. Id del autor: {x.AutorId}");
        }        
    }
}
