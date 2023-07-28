using API.Models;
using Image = SixLabors.ImageSharp.Image;

namespace API.Tools.Validators
{
    public class ImageValidator
    {
        private readonly LibreriaDbContext _db;

        /// <param name="_extensionImage">Extensión permitida para la carga de imágenes</param>
        public static readonly string _extensionImage = ".jpg";

        /// <param name="_maxLength">Tamaño máximo permitido para un archivo en bytes</param>
        public static readonly int _maxLength = 400000;        

        public ImageValidator(LibreriaDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Contiene métodos necesarios para validar la portada cargada
        /// </summary>
        /// <param name="_file">Archivo a validar</param>
        /// <param name="_id">Id del libro para el cual se requiere cargar/actualizar la portada</param>
        public static async Task ValidaImage(IFormFile _file, int _id, LibreriaDbContext _db)
        {
            if (!ValidaIdLibro(_id, _db))
                throw new Exception(MessageFilter.ObtenerMensaje(18));

            if (!ValidaExtension(_file))
                throw new Exception(MessageFilter.ObtenerMensaje(13));

            if (!ValidaArchivoConDatos(_file))
                throw new Exception(MessageFilter.ObtenerMensaje(14));

            if (!ValidaTamaño(_file))
                throw new Exception(MessageFilter.ObtenerMensaje(15));

            bool result = await ValidaArchivo(_file);
            if (!result)
                throw new Exception(MessageFilter.ObtenerMensaje(16));
        }

        /// <summary>
        /// Método que valida que exista el id del libro para el cual se intenta cargar la portada
        /// </summary>
        public static bool ValidaIdLibro(int _id, LibreriaDbContext _db)
        {
            return _db.Libros.Where(l => l.IdLibro == _id).Any() ? true : false;
        }

        /// <summary>
        /// Método que valida la extensión del archivo
        /// </summary>
        /// <param name="_extPermitidas">Array con extensiones permitidas</param>
        public static bool ValidaExtension(IFormFile _file)
        {
            var ext = Path.GetExtension(_file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || _extensionImage != ext)
                return false;

            return true;
        }

        /// <summary>
        /// Método que valida que el archivo contenga datos
        /// </summary>
        public static bool ValidaArchivoConDatos(IFormFile _file)
        {
            if (_file.Length == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Método que valida que el archivo no supere el tamaño máximo permitido
        /// </summary>
        public static bool ValidaTamaño(IFormFile _file)
        {
            if (_file.Length > _maxLength)
                return false;

            return true;
        }

        /// <summary>
        /// Valida que el archivo sea realmente una imagen
        /// </summary>
        public static async Task<bool> ValidaArchivo(IFormFile _file)
        {
            bool result = true;

            try
            {
                // Creamos un stream con los datos de la imagen
                using (var stream = _file.OpenReadStream())
                {
                    // Creamos un memory stream
                    using (var output = new MemoryStream())
                    using (Image image = await Image.LoadAsync(stream))
                    {
                        // Creamos la imagen, en caso de no ser una imagen va a generar una excepción
                        await image.SaveAsync(output, image.Metadata.DecodedImageFormat!);
                    }
                }
            }
            catch
            {
                result = false;
            }
                
            return result;
        }
    }
}
