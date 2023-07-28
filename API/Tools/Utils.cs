using API.Models;
using API.Models.Common;
using API.Tools.Validators;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;

namespace API.Tools
{
    /// <summary>
    /// Contiene métodos comunes entre todo el sitio
    /// </summary>
    public class Utils
    {
        private readonly AppSettings _appSettings;

        public Utils(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Verifica que exista el path del servidor
        /// </summary>
        public static void PathExists(string _librosPath)
        {
            if (!Directory.Exists(_librosPath))
                throw new Exception(MessageFilter.ObtenerMensaje(17));
        }

        /// <summary>
        /// Copia la portada al servidor
        /// </summary>
        /// <param name="_file">Archivo a validar</param>
        /// <param name="_id">Id del libro al cual se le está cargando su porada</param>
        /// <param name="_librosPath">Ruta del servidor donde se guardan las portadas de los libros</param>
        public static async Task CopyFileToServer(IFormFile _file, int _id, string _librosPath)
        {
            try
            {
                var filename = _id + ImageValidator._extensionImage;

                var filePath = Path.Combine(_librosPath, filename);

                using (var stream = File.Create(filePath))
                {
                    await _file.CopyToAsync(stream);
                }
            }
            catch
            {
                throw new Exception(MessageFilter.ObtenerMensaje(22));
            }            
        }

        /// <summary>
        /// Obtiene el token
        /// </summary>
        public static async Task<string> GetToken()
        {
            var _httpContext = new HttpContextAccessor().HttpContext;
            var token = await _httpContext.GetTokenAsync("access_token");
            return token;
        }

        /// <summary>
        /// Devuelve el claim según el tipo
        /// </summary>
        /// /// <param name="_type">Tipo de claim a buscar</param>
        public static async Task<string> GetClaimPorTipo(string _type)
        {
            var token = await GetToken();
            var securityTokenHandler = new JwtSecurityTokenHandler();
            var decriptedToken = securityTokenHandler.ReadJwtToken(token);
            var claims = decriptedToken.Claims;
            var claim = claims.Where(c => c.Type == _type).Any() ? claims.Where(c => c.Type == _type).FirstOrDefault()!.Value : string.Empty;
            
            if (claim == string.Empty)
                throw new Exception(MessageFilter.ObtenerMensaje(21));

            return claim;
        }

        /// <summary>
        /// Verifica que exista el usuario
        /// </summary>
        /// <param name="_user">Email del usuario que envía la solicitud</param>
        public static void VerificaUser(LibreriaDbContext _db, string _user)
        {
            if (!_db.Usuarios.Where(u => u.Email == _user).Any())
                throw new Exception(MessageFilter.ObtenerMensaje(19));
        }
    }
}
