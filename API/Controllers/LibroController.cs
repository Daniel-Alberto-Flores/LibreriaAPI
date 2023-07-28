using API.Models.Response;
using API.Models.ViewModels;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LibroController : Controller
    {
        private ILibroService _libroService;
        private IAutorService _autorService;
        public LibroController(ILibroService libroService, IAutorService autorService)
        {
            _libroService = libroService;
            _autorService = autorService;
        }

        [HttpGet("[action]")]
        public IActionResult GetLibros()
        {
            Response oResponse = new();
            try
            {
                var list = _libroService.GetLibros();
                oResponse.Data = list;
                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);
        }

        [HttpGet("[action]/{_id}")]
        public IActionResult GetLibro(int _id)
        {
            Response oResponse = new();
            try
            {
                var oLibro = _libroService.GetLibro(_id);
                oResponse.Data = oLibro;
                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] LibroViewModel _model)
        {
            Response oResponse = new();
            try
            {
                _libroService.Add(_model);

                var autor = _autorService.GetAutorPorId(_model.AutorId);
                var autorNombreApellido = autor.Nombre + autor.Apellido;

                oResponse.Data = _libroService.GetLibroPorNombreAutor(_model.Nombre!, autorNombreApellido);
                oResponse.Success = 1;
                oResponse.Message = Tools.MessageFilter.ObtenerMensaje(7);
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);
        }

        [HttpPut]
        public IActionResult Edit([FromBody] LibroViewModel _model)
        {
            Response oResponse = new();
            try
            {
                _libroService.Edit(_model);
                oResponse.Success = 1;
                oResponse.Message = Tools.MessageFilter.ObtenerMensaje(8);
            }
            catch (Exception ex)
            {
                oResponse.Success = 0;
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);
        }
    }
}
