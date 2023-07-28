using API.Models.Request;
using API.Models.Response;

namespace API.Services
{
    public interface IUsuarioService
    {
        UsuarioResponse Auth(AuthRequest model);
    }
}
