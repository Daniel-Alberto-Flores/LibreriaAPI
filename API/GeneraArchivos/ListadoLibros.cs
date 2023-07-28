using API.Models;
using Spire.Xls;
using System;
using System.Drawing;
using Color = System.Drawing.Color;

namespace API.GeneraArchivos
{
    public class ListadoLibros
    {
        public static Byte[] GeneraArchivos(List<Libro> _listLibros)
        {            
            try
            {
                //Creates workbook
                Workbook workbook = new Workbook();
                workbook.Version = ExcelVersion.Version2016;

                workbook.Worksheets[2].Remove();
                workbook.Worksheets[1].Remove();

                Worksheet oWSheet = workbook.Worksheets[0];
                oWSheet.Name = "Listado de libros";

                DateTime oDate = DateTime.Now;

                // Colores:
                Color colorAzul = Color.FromArgb(42, 96, 153);
                Color gris = Color.FromArgb(222, 230, 239);

                // Aplicamos estilo a las filas 1 y 2
                oWSheet.Range["A1:D1"].Merge();
                oWSheet.Range["A1:D2"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                oWSheet.Range["A1:D2"].Style.Font.Size = 14;
                oWSheet.Range["A1:D2"].Style.Font.Color = Color.White;
                oWSheet.Range["A1:D2"].Style.Color = colorAzul;

                // Cargamos el texto de las filas 1 y 2
                oWSheet.Range["A1"].Text = "Listado de libros";
                oWSheet.Range["A2"].Text = "Fecha:";
                oWSheet.Range["B2"].Text = oDate.ToString("dd/MM/yyyy");
                oWSheet.Range["C2"].Text = "Hora:";
                oWSheet.Range["D2"].Text = oDate.ToString("HH:mm:ss");

                // Header
                // Estilo
                oWSheet.Range["A3:D3"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                oWSheet.Range["A3:D3"].Style.Font.IsBold = true;
                oWSheet.Range["A3:D3"].Style.Color = gris;
                // Estilo

                // Carga de datos
                oWSheet.Range["A3"].Text = "IdLibro";
                oWSheet.Range["B3"].Text = "Nombre";
                oWSheet.Range["C3"].Text = "Año de publicación";
                oWSheet.Range["D3"].Text = "Autor";
                // Carga de datos
                // Header

                int contaRow = 4;// Lo utilizamos para mantener el valor de la última fila con datos
                // Cargamos el listado de libros
                foreach (var oLibro in _listLibros)
                {
                    // Carga de datos
                    oWSheet.Range["A" + contaRow].Text = oLibro.IdLibro.ToString();
                    oWSheet.Range["B" + contaRow].Text = oLibro.Nombre;
                    oWSheet.Range["C" + contaRow].Text = oLibro.Publicacion.ToString();
                    oWSheet.Range["D" + contaRow].Text = $"{oLibro.Autor.Nombre} {oLibro.Autor.Apellido}";

                    // Aplicamos los estilos
                    oWSheet.Range["A" + contaRow].Style.HorizontalAlignment = HorizontalAlignType.Right;
                    oWSheet.Range["B" + contaRow].Style.HorizontalAlignment = HorizontalAlignType.Left;
                    oWSheet.Range[$"C{contaRow}:D{contaRow}"].Style.HorizontalAlignment = HorizontalAlignType.Center;
                    contaRow++;
                }
                // Cargamos el listado de libros

                // Aplicamos borde a todo el documento
                oWSheet.Range["A1:D" + (contaRow-1)].BorderInside(LineStyleType.Thin, Color.Black);
                oWSheet.Range["A1:D" + (contaRow-1)].BorderAround(LineStyleType.Thin, Color.Black);

                // Ajustamos las columnas y filas a su contenido para que se pueda visualizar correctamente
                oWSheet.AllocatedRange.AutoFitColumns();
                oWSheet.AllocatedRange.AutoFitRows();

                // Guardamos el archivo generado en un memory Stream
                byte[] byteArray;
                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.SaveToStream(ms);
                    byteArray = ms.ToArray();
                }
                return byteArray;
            }
            catch
            {
                throw new Exception(Tools.MessageFilter.ObtenerMensaje(10));
            }
        }
    }
}
