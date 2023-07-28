using API.Models;
using API.Models.Request;
using API.Models.Response;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {        
        private IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService iusuarioService, LibreriaDbContext db)
        {
            _usuarioService = iusuarioService;            
        }

        [HttpPost("login")]
        public IActionResult Autentificar([FromBody] AuthRequest model)
        {
            Response response = new();

            var userResponse = _usuarioService.Auth(model);

            if (userResponse != null)
            {
                response.Success = 1;
                response.Data = userResponse;                
            }
            else
            {
                response.Success = 0;
                response.Message = "Usuario o contraseña incorrecto.";
            }           

            return Ok(response);
        }
    }
}
