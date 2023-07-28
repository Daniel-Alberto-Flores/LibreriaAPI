using API.Tools.Validators;

namespace API.Tools
{
    public class MessageFilter
    {
        public static string ObtenerMensaje(int codError)
        {
            var oMensaje = string.Empty;
            try
            {
                switch (codError)
                {
                    case 1:
                        oMensaje = "Se ha producido un error al obtener el listado de libros.";
                        break;
                    case 2:
                        oMensaje = "Se ha producido un error al obtener el libro ingresado.";
                        break;
                    case 3:
                        oMensaje = "No existe el autor ingresado.";
                        break;
                    case 4:
                        oMensaje = "Se ha producido un error al añadir el libro ingresado.";
                        break;
                    case 5:
                        oMensaje = "No existe el IdLibro ingresado";
                        break;
                    case 6:
                        oMensaje = "Se ha producido un error al editar el libro ingresado.";
                        break;
                    case 7:
                        oMensaje = "Libro insertado correctamente.";
                        break;
                    case 8:
                        oMensaje = "Libro actualizado correctamente.";
                        break;
                    case 9:
                        oMensaje = "No ha producido un error al obtener el listado de libros. No se van a poder exportar los datos, reintente nuevamente más tarde.";
                        break;
                    case 10:
                        oMensaje = "Se ha producido un error al intentar generar el listado solicitado. Reintente nuevamente más tarde.";
                        break;
                    case 11:
                        oMensaje = "Archivo exportado correctamente.";
                        break;
                    case 12:
                        oMensaje = "Portada de libro añadida correctamente.";
                        break;
                    case 13:
                        oMensaje = $"Extensión de imagen inválida. Solo se permiten las extensiones {string.Join(",", ImageValidator._extensionImage)}.";
                        break;
                    case 14:
                        oMensaje = "Archivo cargado sin datos.";
                        break;
                    case 15:
                        oMensaje = $"El archivo cargado no puede superar los {ImageValidator._maxLength/1000} kb";
                        break;
                    case 16:
                        oMensaje = "El archivo cargado no es una imagen.";
                        break;
                    case 17:
                        oMensaje = "No existe la ruta del servidor asignada para guardar las portadas de los libros.";
                        break;
                    case 18:
                        oMensaje = "No existe el id del libro para el cual se intenta cargar/actualizar la portada.";
                        break;
                    case 19:
                        oMensaje = "No existe el usuario ingresado.";
                        break;
                    case 20:
                        oMensaje = "No se pudo loguear los cambios realizados al importar la portada en el servidor.";
                        break;
                    case 21:
                        oMensaje = "No existe la claim buscada.";
                        break;
                    case 22:
                        oMensaje = "Se ha producido un error al intentar copiar la portada al servidor.";
                        break;
                    default:
                        oMensaje = "Se ha producido un error inesperado al procesar su solicitud";
                        break;
                }
            }
            catch
            {

            }
            return oMensaje;
        }
    }
}
