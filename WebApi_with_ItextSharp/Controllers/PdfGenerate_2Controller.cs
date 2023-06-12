using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using System.Net;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfGenerate_2Controller : ControllerBase
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PdfGenerate_2Controller(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("GeneratePdf")]
        public IActionResult GeneratePdf()
        {
            try
            {
                /// Creating a A4 size doc
                var pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 20f);

                /*var fileName = $"Product-{DateTime.Now.ToFileTimeUtc()}.pdf";

                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", fileName);*/


                var fileName = $"student-{DateTime.Now.ToFileTimeUtc()}.pdf";
                var fileBasePath = $"{_webHostEnvironment.WebRootPath}\\Files";
                var filePath = $"{fileBasePath}\\{fileName}";

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    var writer = PdfWriter.GetInstance(pdfDoc, fs);

                    var labelFont_9 = FontFactory.GetFont(FontFactory.HELVETICA, 9, new BaseColor(76, 76, 76));
                    var labelFont_9_Bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, new BaseColor(76, 76, 76));


                    pdfDoc.Open();

                    /// Adding Image
                    /* var p = new Paragraph();
                     var imgPath = $"{_webHostEnvironment.WebRootPath}\\images\\Gimp.png";
                     var img = Image.GetInstance("default.png");
                     img.ScaleToFit(100, 100);
                     p.Add(new Chunk(img, 20, 0, true));
                     pdfDoc.Add(p);

                     /// Adding the Paragraph
                     p = new Paragraph(new Chunk("Imaginary Company", labelFont_9_Bold));
                     p.IndentationLeft = 40f;
                     p.SpacingBefore = -35f;
                     pdfDoc.Add(p);

                     p = new Paragraph(new Chunk("PDF is created with iTextSharp", labelFont_9));
                     p.IndentationLeft = 40f;
                     pdfDoc.Add(p);*/
                   var p = new Paragraph(new Chunk("Students", labelFont_9));
                    p.IndentationLeft = 200f;
                    pdfDoc.Add(p);


                    /// Creating a table
                    var productTable = new PdfPTable(6);
                    productTable.SpacingBefore = 20f;

                    productTable.SetTotalWidth(new float[] { 50f, 100f, 100f, 50f,50f,50f });
                    // productTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    //productTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    //productTable.DefaultCell.Border = Rectangle.BOTTOM_BORDER;


                    /// Creating headers 
                    productTable.AddCell(CustomAlignMethod_BorderBottom("Id", labelFont_9_Bold, Element.ALIGN_LEFT));
                    productTable.AddCell(CustomAlignMethod_BorderBottom("FullName", labelFont_9_Bold, Element.ALIGN_LEFT));
                    productTable.AddCell(CustomAlignMethod_BorderBottom("Email", labelFont_9_Bold, Element.ALIGN_LEFT));
                    productTable.AddCell(CustomAlignMethod_BorderBottom("Age", labelFont_9_Bold, Element.ALIGN_LEFT));
                    productTable.AddCell(CustomAlignMethod_BorderBottom("BirthDate", labelFont_9_Bold, Element.ALIGN_LEFT));
                    productTable.AddCell(CustomAlignMethod_BorderBottom("Address", labelFont_9_Bold, Element.ALIGN_LEFT));


                    /// Creating Body
                    foreach (var student in _dbContext.Students.ToList())
                    {
                        productTable.AddCell(CustomAlignMethod_BorderBottom($"{student.Id}", labelFont_9, Element.ALIGN_LEFT));
                        productTable.AddCell(CustomAlignMethod_BorderBottom($"{student.FullName}", labelFont_9, Element.ALIGN_LEFT));
                        productTable.AddCell(CustomAlignMethod_BorderBottom($"{student.Email}", labelFont_9, Element.ALIGN_LEFT));
                        productTable.AddCell(CustomAlignMethod_BorderBottom($"{student.Age}", labelFont_9, Element.ALIGN_LEFT));
                        productTable.AddCell(CustomAlignMethod_BorderBottom($"{student.BirthDate.ToString("dd.MM.yyyy")}", labelFont_9, Element.ALIGN_LEFT));
                        productTable.AddCell(CustomAlignMethod_BorderBottom($"{student.Address}", labelFont_9, Element.ALIGN_LEFT));
                    }// end foreach

                    pdfDoc.Add(productTable);



                    /// Closing the file and pdf instances 
                    pdfDoc.Close();
                    writer.Close();
                    fs.Close();


                }// end using{}

                var path = Path.Combine(_webHostEnvironment.WebRootPath, "Files", fileName);

                byte[] fileBytes = System.IO.File.ReadAllBytes(Url.Content(path));

                return File(fileBytes, "application/pdf", fileName);

            }
            catch (Exception ex)
            {
                return BadRequest("Error: "+ex.Message);
            }
        }
        protected PdfPCell CustomAlignMethod_BorderBottom(string textText, Font fontStyle, int hroizontalAlignment)
        {
            var cell = new PdfPCell(new Phrase(textText, fontStyle));
            cell.UseVariableBorders = true;

            cell.BorderColorLeft = BaseColor.WHITE;
            cell.BorderColorRight = BaseColor.WHITE;
            cell.BorderColorTop = BaseColor.WHITE;
            cell.BorderColorBottom = BaseColor.GREEN;
            cell.HorizontalAlignment = hroizontalAlignment;

            return cell;

        }

        protected byte[] GetFile(string fileName)
        {
            var result = System.IO.File.ReadAllBytes(WebUtility.UrlDecode(fileName));
            return result;
        }
    }
}
