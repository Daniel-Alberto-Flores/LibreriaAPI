using API.Models.Response;
using API.Services;
using API.Tools;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private IUploadService _uploadService;


        /// <summary>
        /// Controller encargado de subir los archivos al servidor
        /// </summary>
        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadImageAsync([FromForm] IFormFile _image, [FromForm] int _id)
        {
            Response oResponse = new();
            try
            {
                await _uploadService.UploadImage(_image, _id);

                oResponse.Success = 1;
                oResponse.Message = MessageFilter.ObtenerMensaje(12);
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
