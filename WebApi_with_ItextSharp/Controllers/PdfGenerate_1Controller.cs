using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebApi.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfGenerate_1Controller : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PdfGenerate_1Controller(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("CreatePdf")]
        public FileResult CreatePdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            string strPDFFileName = string.Format("Student.pdf");
            Document doc = new Document();
            doc.SetMargins(0, 0, 0, 0);
            PdfPTable tableLayout = new PdfPTable(6);
            doc.SetMargins(10, 10, 10, 0);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontInvoice = new iTextSharp.text.Font(bf, 20, iTextSharp.text.Font.NORMAL);
            Paragraph paragraph = new Paragraph("Student Details", fontInvoice);
            paragraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph);
            Paragraph p3 = new Paragraph();
            p3.SpacingAfter = 6;
            doc.Add(p3);
            doc.Add(Add_Content_To_PDF(tableLayout));
            doc.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            return File(workStream, "application/pdf", strPDFFileName);
        }

        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {
            float[] headers = { 5, 25, 30, 30 ,8,15}; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            var count = 1;
            //Add header  
            AddCellToHeader(tableLayout, "Id");
            AddCellToHeader(tableLayout, "FullName");
            AddCellToHeader(tableLayout, "Address");
            AddCellToHeader(tableLayout, "Email");
            AddCellToHeader(tableLayout, "Age");
            AddCellToHeader(tableLayout, "BirthDate");

            foreach (var cust in _dbContext.Students.ToList())
            {
                if (count >= 1)
                {
                    //Add body  
                    AddCellToBody(tableLayout, cust.Id.ToString(), count);
                    AddCellToBody(tableLayout, cust.FullName, count);
                    AddCellToBody(tableLayout, cust.Address.ToString(), count);
                    AddCellToBody(tableLayout, cust.Email.ToString(), count);
                    AddCellToBody(tableLayout, cust.Age.ToString(), count);
                    AddCellToBody(tableLayout, cust.BirthDate.ToString("dd.MM.yyyy"), count);
                    
                    count++;
                }
            }
            return tableLayout;
        }

        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 8,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }
        [HttpGet("rere")]
        private static void AddCellToBody(PdfPTable tableLayout, string cellText, int count)
        {
            if (count % 2 == 0)
            {
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.BLACK)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 8,
                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                });
            }
            else
            {
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 8,
                    BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211)
                });
            }
        }
    }
}
