using API.Models;
using API.Models.Common;
using API.Tools;
using API.Tools.Validators;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Transactions;

namespace API.Services
{
    public class UploadService : IUploadService
    {
        private readonly AppSettings _appSettings;
        private readonly LibreriaDbContext _db;
        public UploadService(IOptions<AppSettings> appSettings, LibreriaDbContext db)
        {
            _appSettings = appSettings.Value;
            _db = db;
        }

        public async Task UploadImage(IFormFile _file, int _id)
        {
            try
            {
                await ImageValidator.ValidaImage(_file, _id, _db);

                var user = await Utils.GetClaimPorTipo("email");

                Utils.VerificaUser(_db, user);

                string path = _appSettings.LibrosPath ?? string.Empty;
                Utils.PathExists(path);

                await Utils.CopyFileToServer(_file, _id, path);                

                LogUpload(_id, user);
            }
            catch (Exception)
            {
                throw;
            }            
        }

        public void LogUpload(int _id, string _user)
        {
            try
            {
                var datosUsuario = _db.Usuarios.Where(u => u.Email == _user).FirstOrDefault();
                var upload = new Upload()
                {
                    Descripcion = $"El usuario {datosUsuario!.Nombre} actualizó la portada del libro {_id}.",
                    UsuarioId = datosUsuario.Id,
                    FechaDeModificacion = DateTime.Now
                };

                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    _db.Uploads.Add(upload);
                    _db.SaveChanges();
                    scope.Complete();
                }
            }
            catch
            {
                throw new Exception(MessageFilter.ObtenerMensaje(20));
            }
        }
    }
}
