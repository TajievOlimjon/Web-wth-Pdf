using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;

namespace WebApi_with_QuestPDF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public GenerateController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("GeneratePdf")]
        public IActionResult GeneratePdf()
        {
            var document = Document.Create(container =>
            {

                container.Page(page =>
                {
                    page.Margin(10);

                    page.Header().Element(Header);

                    page.Content().Element(ComposeTable);

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            });

            /*var fontFemaly = "C:\\Users\\Developer\\source\\repos\\Web-with-Pdf\\WebApi_with_QuestPDF\\Fonts\\ArialRegular.ttf";
            FontManager.RegisterFont(System.IO.File.OpenRead(fontFemaly));*/

            var filename = $"product.{Guid.NewGuid()}.pdf";
            var path = Path.Combine("C:\\Users\\Developer\\source\\repos\\Web-with-Pdf\\WebApi_with_QuestPDF\\PdfFiles\\", filename);


            //document.GeneratePdf(path);

            void Header(IContainer container)
            {
                container.Row(row =>
                {
                    row.Spacing(25);

                    row.ConstantItem(100).Image("C:\\Users\\Developer\\source\\repos\\Web-with-Pdf\\WebApi_with_QuestPDF\\Images\\SoftClub.Logo.jpg");

                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Text("SoftClub Academy")
                        .FontSize(36)
                        .FontColor(Colors.Orange.Medium)
                        .SemiBold();
                    });
                });
            }
            void ComposeTable(IContainer container)
            {
                container.Table(table =>
                {
                    // step 1
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(25);
                        columns.RelativeColumn();
                        columns.ConstantColumn(90);
                        columns.ConstantColumn(90);
                        columns.ConstantColumn(40);
                        columns.ConstantColumn(40);
                        columns.ConstantColumn(100);
                    });

                    // step 2
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("#");
                        header.Cell().Element(CellStyle).Text("Product name");
                        header.Cell().Element(CellStyle).AlignLeft().Text("Description");
                        header.Cell().Element(CellStyle).AlignRight().Text("Publish Date");
                        header.Cell().Element(CellStyle).AlignRight().Text("Qty");
                        header.Cell().Element(CellStyle).AlignRight().Text("Price");
                        header.Cell().Element(CellStyle).AlignCenter().Text("Category name");

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                        }
                    });

                    var products = _dbContext.Products.ToList();
                    foreach (var product in products)
                    {
                        table.Cell().Element(CellStyle).Text(product.Id);
                        table.Cell().Element(CellStyle).Text(product.Name).FontFamily("Arial");
                        table.Cell().Element(CellStyle).AlignCenter().Text(product.Description).FontFamily("Arial");
                        table.Cell().Element(CellStyle).AlignRight().Text(product.PublishDate.ToString("dd.MM.yyyy"));
                        table.Cell().Element(CellStyle).AlignRight().Text(product.Quantity);
                        table.Cell().Element(CellStyle).AlignRight().Text(product.Price);
                        table.Cell().Element(CellStyle).AlignCenter().Text(product.CategoryName).FontFamily("Arial");

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                        }

                    }

                });
            }

            //return Ok("Success");

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);

            return File(stream, "application/pdf", filename);
        }
    }
}
