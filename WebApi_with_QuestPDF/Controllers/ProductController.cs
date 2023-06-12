using Microsoft.AspNetCore.Mvc;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace WebApi_with_QuestPDF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public IActionResult GeneratePdf()
        {
            var document = Document.Create(container =>
              {
                  
                  container.Page(page =>
                  {
                      page.Margin(50);

                      page.Header().Element(ComposeHeader);

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

            document.GeneratePdf(path);

            void ComposeHeader(IContainer container)
            {
                var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                container.Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Text($"Invoice ").Style(titleStyle);

                        column.Item().Text(text =>
                        {
                            text.Span("Issue date: ").SemiBold();
                            text.Span($"{DateTime.Now.Date:d}");
                        });

                        column.Item().Text(text =>
                        {
                            text.Span("Due date: ").SemiBold();
                            text.Span($"{DateTime.Now.Date:d}");
                        });
                    });

                    row.ConstantItem(100).Height(50).Placeholder();
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
                        columns.RelativeColumn(3);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    // step 2
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("#");
                        header.Cell().Element(CellStyle).Text("Product");
                        header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
                        header.Cell().Element(CellStyle).AlignRight().Text("Quantity");
                        header.Cell().Element(CellStyle).AlignRight().Text("Total");

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                        }
                    });

                    var cnt = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

                    foreach (int i in cnt)
                    {
                        table.Cell().Element(CellStyle).Text(i);
                        table.Cell().Element(CellStyle).Text("Самсунг 8 A ").FontFamily("Arial");
                        table.Cell().Element(CellStyle).AlignRight().Text($"{120}$");
                        table.Cell().Element(CellStyle).AlignRight().Text(5*i);
                        table.Cell().Element(CellStyle).AlignRight().Text($"{120 * 5*i}$");

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                        }

                    }
                    
                });
            }
          
            return Ok();
        }
    }
}
