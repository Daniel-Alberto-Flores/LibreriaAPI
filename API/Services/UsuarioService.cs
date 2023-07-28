using API.Models.Common;
using API.Models;
using API.Models.Request;
using API.Models.Response;
using API.Tools;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly LibreriaDbContext _db;
        private readonly AppSettings _appSettings;

        public UsuarioService(IOptions<AppSettings> appSettings, LibreriaDbContext db)
        {
            _appSettings = appSettings.Value;
            _db = db;
        }

        public UsuarioResponse Auth(AuthRequest model)
        {
            UsuarioResponse usuarioResponse = new();

            string spassword = Encrypt.GetSHA256(model.Password);

            var usuario = _db.Usuarios.Where(u => u.Email == model.Email &&
                                                  u.Password == spassword).FirstOrDefault();
            if (usuario == null) return null;

            usuarioResponse.Email = usuario.Email;
            usuarioResponse.Token = GetToken(usuario);

            return usuarioResponse;
        }

        private string GetToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                        new Claim(ClaimTypes.Email, usuario.Email)
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
