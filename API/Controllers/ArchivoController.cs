using API.Models;
using API.Models.Response;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArchivoController : ControllerBase
    {
        private IArchivoService _archivoService;
        private ILibroService _libroService;

        /// <summary>
        /// Controller encargado de generar los archivos descargables
        /// </summary>
        public ArchivoController(IArchivoService archivoService, ILibroService libroService)
        {
            _archivoService = archivoService;
            _libroService = libroService;
        }        

        [HttpGet("[action]")]
        public IActionResult GeneraListadoLibros()
        {
            Response oResponse = new();
            try
            {
                var listLibros = (List<Libro>)_libroService.GetLibros();

                if (listLibros != null)
                {
                    byte[] byteArray = _archivoService.GeneraListadoLibros(listLibros);
                    oResponse.Data = byteArray;
                    oResponse.Success = 1;
                    oResponse.Message = Tools.MessageFilter.ObtenerMensaje(11);
                }
                else
                {
                    oResponse.Message = Tools.MessageFilter.ObtenerMensaje(9);
                    oResponse.Success = 0;
                }                
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
