using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SistemaDeViajes.Filters;
using SistemaDeViajes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaDeViajes.Controllers
{
    [Acceso]
    public class MarcaController : Controller
    {
        BDPasajeEntities db;
        public MarcaController()
        {
            db = new BDPasajeEntities();
        }

        public FileResult ExcelMarcas()
        {
            byte[] buffer;

            using (MemoryStream ms = new MemoryStream())
            {
                //Creamos todo el documento
                ExcelPackage ep = new ExcelPackage();

                //Creamos una hoja
                ep.Workbook.Worksheets.Add("Reporte");
                ExcelWorksheet ew = ep.Workbook.Worksheets[1];

                //Ponemos nombres de las columnas
                ew.Cells[1, 1].Value = "ID";
                ew.Cells[1, 2].Value = "Nombre";
                ew.Cells[1, 3].Value = "Descripción";

                //Definimos los anchos
                ew.Column(1).Width = 20;
                ew.Column(2).Width = 40;
                ew.Column(3).Width = 180;

                //Definimos estilos a las cabeceras
                using (var range = ew.Cells[1, 1, 1, 3])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                }

                //Llenamos el contenido de la tabla
                List<MarcaModel> listModel = (List<MarcaModel>)Session["Lista"];
                int numRegistros = listModel.Count;
                for (int i = 0; i < numRegistros; i++)
                {
                    ew.Cells[i + 2, 1].Value = listModel[i].IDMarca;
                    ew.Cells[i + 2, 2].Value = listModel[i].Nombre;
                    ew.Cells[i + 2, 3].Value = listModel[i].Descripcion;

                }

                ep.SaveAs(ms);
                buffer = ms.ToArray();

            }

            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }



        class HeaderFooter : PdfPageEventHelper
        {
            string pathImageGlobal = null;
            string pathImageFondo = null;
            public HeaderFooter(string pathLogo, string marcaAgua)
            {
                pathImageGlobal = pathLogo;
                pathImageFondo = marcaAgua;
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.Image fondo = iTextSharp.text.Image.GetInstance(pathImageFondo);

                float positionY = (writer.PageSize.Top /2) - (fondo.Width/2);
                float positionX = (writer.PageSize.Right/2) - (fondo.Height/2);

                fondo.SetAbsolutePosition(positionX, positionY);
                PdfGState state = new PdfGState();
                state.FillOpacity = 0.3f;
                cb.SetGState(state);
                cb.AddImage(fondo);

                //base.OnEndPage(writer, document);
                PdfPTable header = new PdfPTable(3);
                header.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                header.DefaultCell.Border = 0;

                header.AddCell(new Paragraph());
                PdfPCell _cell = new PdfPCell(new Paragraph("Lista de marcas"));
                _cell.HorizontalAlignment = Element.ALIGN_CENTER;
                _cell.Border = 0;
                header.AddCell(_cell);
                header.AddCell(new Paragraph());

                header.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetTop(document.TopMargin) + 40, writer.DirectContent);

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                PdfPTable footer = new PdfPTable(3);
                footer.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
                footer.DefaultCell.Border = 0;

                footer.AddCell(new Paragraph());
                _cell = new PdfPCell(new Paragraph("NovoSys"));
                _cell.HorizontalAlignment = Element.ALIGN_CENTER;
                _cell.Border = 0;
                footer.AddCell(_cell);

                _cell = new PdfPCell(new Paragraph("Pagina " + writer.PageNumber));
                _cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _cell.Border = 0;

                footer.AddCell(_cell);
                footer.AddCell(new Paragraph());
                footer.WriteSelectedRows(0, -1, document.LeftMargin, writer.PageSize.GetBottom(document.BottomMargin) -5, writer.DirectContent);


                //Imagen
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(pathImageGlobal);
                logo.SetAbsolutePosition(document.LeftMargin, writer.PageSize.GetTop(document.TopMargin) + 2);
                logo.ScaleAbsolute(50f, 50f);
                document.Add(logo);

            }
        }

        public ActionResult PDFMarcasNew()
        {
            MemoryStream ms = new MemoryStream();

            Document document = new Document(iTextSharp.text.PageSize.LETTER, 30f, 20f, 50f, 40f);
            PdfWriter pw = PdfWriter.GetInstance(document, ms);

            string pathImageGlobal = Server.MapPath("/img/NOVOLEADSlogoo.png");
            string pathImageFondo = Server.MapPath("/img/NOVOLEADSlogoo.png");
            pw.PageEvent = new HeaderFooter(pathImageGlobal, pathImageFondo);
            document.Open();

            string nameFont = Server.MapPath("/fonts/Megan June.otf");

            BaseFont bf = BaseFont.CreateFont(nameFont, BaseFont.CP1250, BaseFont.EMBEDDED);
            iTextSharp.text.Font fontText = new iTextSharp.text.Font(bf, 10, 0, BaseColor.BLACK);


            PdfPTable table = new PdfPTable(3);
            table.WidthPercentage = 100f;


            PdfPCell celda1 = new PdfPCell(new Phrase("ID"));
            celda1.BackgroundColor = new BaseColor(130, 130, 130);
            celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(celda1);

            PdfPCell celda2 = new PdfPCell(new Phrase("Nombre"));
            celda2.BackgroundColor = new BaseColor(130, 130, 130);
            celda2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(celda2);

            PdfPCell celda3 = new PdfPCell(new Phrase("Descripción"));
            celda3.BackgroundColor = new BaseColor(130, 130, 130);
            celda3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            table.AddCell(celda3);


            List<MarcaModel> listModel = (List<MarcaModel>)Session["Lista"];
            int numregistros = listModel.Count;

            for (int i = 0; i < numregistros; i++)
            {
                PdfPCell _cell = new PdfPCell();

                _cell = new PdfPCell(new Paragraph(listModel[i].IDMarca.ToString(), fontText));
                _cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(_cell);

                _cell = new PdfPCell(new Paragraph(listModel[i].Nombre, fontText));
                _cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(_cell);

                _cell = new PdfPCell(new Paragraph(listModel[i].Descripcion, fontText));
                _cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(_cell);
            }

            document.Add(table);
            
            document.Close();

            byte[] byteStream = ms.ToArray();
            ms = new MemoryStream();
            ms.Write(byteStream, 0, byteStream.Length);
            ms.Position = 0;


            return new FileStreamResult(ms, "application/pdf");
            //return null;

        }




        public FileResult PDFMarcas()
        {
            Document doc = new Document();
            byte[] buffer;
            //using (MemoryStream ms = new MemoryStream())
            //{
            MemoryStream ms = new MemoryStream();
                PdfWriter pw = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                //Paragraph title = new Paragraph("Lista Marca");
                //title.Alignment = Element.ALIGN_CENTER;
                //doc.Add(title);

                //Paragraph space = new Paragraph(" ");
                //doc.Add(space);

                //Definimos las columnas de la tabla
                PdfPTable tabla = new PdfPTable(3);

                //Anchos de las columnas
                float[] values = new float[3] { 30, 40, 80 };

                //Agignacion de anchos
                tabla.SetWidths(values);

                //DEfinimos las celdas (contenido de la tabla), los colores y el alineado al centro
                PdfPCell celda1 = new PdfPCell(new Phrase("ID"));
                celda1.BackgroundColor = new BaseColor(130, 130, 130);
                celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda1);

                PdfPCell celda2 = new PdfPCell(new Phrase("Nombre"));
                celda2.BackgroundColor = new BaseColor(130, 130, 130);
                celda2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda2);

                PdfPCell celda3 = new PdfPCell(new Phrase("Descripción"));
                celda3.BackgroundColor = new BaseColor(130, 130, 130);
                celda3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tabla.AddCell(celda3);

                List<MarcaModel> listModel = (List<MarcaModel>)Session["Lista"];
                int numregistros = listModel.Count;

                for (int i = 0; i < numregistros; i++)
                {
                    tabla.AddCell(listModel[i].IDMarca.ToString());
                    tabla.AddCell(listModel[i].Nombre);
                    tabla.AddCell(listModel[i].Descripcion);
                }

                doc.Add(tabla);
                doc.Close();

                buffer = ms.ToArray();
            //}

            return File(buffer, "application/pdf");
        }

        // GET: Marca
        public ActionResult Index(MarcaModel oModel)
        {
            string nombre = oModel.Nombre;
            List<MarcaModel> model = null;

            if (nombre == null)
            {
                model = (from m in db.Marca
                         where m.BHABILITADO == 1
                         select new MarcaModel
                         {
                             IDMarca = m.IIDMARCA,
                             Nombre = m.NOMBRE,
                             Descripcion = m.DESCRIPCION
                         }).ToList();
                Session["Lista"] = model;
            }
            else
            {
                model = (from m in db.Marca
                         where m.BHABILITADO == 1 && m.NOMBRE.Contains(nombre)
                         select new MarcaModel
                         {
                             IDMarca = m.IIDMARCA,
                             Nombre = m.NOMBRE,
                             Descripcion = m.DESCRIPCION
                         }).ToList();

                Session["Lista"] = model;
            }

            return View(model);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(MarcaModel model)
        {
            int registrosEncontrados = 0;
            string marca = model.Nombre;

            registrosEncontrados = db.Marca.Where(p => p.NOMBRE == marca).Count();

            if (!ModelState.IsValid || registrosEncontrados >= 1)
            {
                if (registrosEncontrados >= 1)
                {
                    model.msjError = "El nombre de la marca ya existe";
                }
                return View(model);
            }
            else
            {
                Marca oMarca = new Marca();
                oMarca.NOMBRE = model.Nombre;
                oMarca.DESCRIPCION = model.Descripcion;
                oMarca.BHABILITADO = 1;
                db.Marca.Add(oMarca);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            MarcaModel model = new MarcaModel();
            Marca oMarca = db.Marca.Where(p => p.IIDMARCA == id).First();

            model.IDMarca = oMarca.IIDMARCA;
            model.Nombre = oMarca.NOMBRE;
            model.Descripcion = oMarca.DESCRIPCION;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(MarcaModel model)
        {
            int numRegEncontrados = 0;
            string marca = model.Nombre;
            int idMarca = model.IDMarca;

            numRegEncontrados = db.Marca.Where(p => p.NOMBRE == marca && p.IIDMARCA != idMarca).Count();

            if (!ModelState.IsValid || numRegEncontrados >= 1)
            {
                if (numRegEncontrados >= 1)
                {
                    model.msjError = "El nombre de la marca ya existe";
                }
                return View(model);
            }
            else
            {
                Marca oMarca = db.Marca.Where(p => p.IIDMARCA == idMarca).First();
                oMarca.NOMBRE = model.Nombre;
                oMarca.DESCRIPCION = model.Descripcion;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Marca oMarca = db.Marca.Where(p => p.IIDMARCA == id).First();
            oMarca.BHABILITADO = 0;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
