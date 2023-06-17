using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using SkiaSharp;

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

        [HttpGet("Grid")]
        public IActionResult GridPdf()
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(1, Unit.Centimetre);

                    page.Content().Grid(grid =>
                    {
                        grid.VerticalSpacing(15);
                        grid.HorizontalSpacing(15);
                        grid.AlignCenter();
                        grid.Columns(10); // 12 by default

                        grid.Item(6).Background(Colors.Blue.Lighten1).Height(50);
                        grid.Item(4).Background(Colors.Blue.Lighten3).Height(50);

                        grid.Item(2).Background(Colors.Teal.Lighten1).Height(70);
                        grid.Item(3).Background(Colors.Teal.Lighten2).Height(70);
                        grid.Item(5).Background(Colors.Teal.Lighten3).Height(70);

                        grid.Item(2).Background(Colors.Green.Lighten1).Height(50);
                        grid.Item(2).Background(Colors.Green.Lighten2).Height(50);
                        grid.Item(2).Background(Colors.Green.Lighten3).Height(50);
                    });
                });
                container.Page(page =>
                {
                    var fonts = new[]
                    {
                        Fonts.Calibri,
                        Fonts.Candara,
                        Fonts.Arial,
                        Fonts.TimesNewRoman,
                        Fonts.Consolas,
                        Fonts.Tahoma,
                        Fonts.Impact,
                        Fonts.Trebuchet,
                        Fonts.ComicSans
                    };
                    page.Content()
                        .Padding(25).Grid(grid =>
                        {
                            grid.Columns(3);

                            foreach (var font in fonts)
                            {
                                grid.Item()
                                    .Border(1)
                                    .BorderColor(Colors.Grey.Medium)
                                    .Padding(10)
                                    .Text(font)
                                    .FontFamily(font).FontSize(16);
                            }
                        });
                });
            });

            var filename = $"Text.pdf";
            var path = Path.Combine("C:\\Users\\Developer\\source\\repos\\Web-with-Pdf\\WebApi_with_QuestPDF\\PdfFiles\\", filename);

            document.GeneratePdf(path);

            return Ok(new { Code = 200 });
        }
        [HttpGet("Report2GeneratePdfFile")]
        private IActionResult Report2GeneratePdfFile()
        {
            var document = Document.Create(container =>
            {
                var pageSizes = new List<(string name, double width, double height)>()
                {
                    ("Letter (ANSI A)", 8.5f, 11),
                    ("Legal", 8.5f, 14),
                    ("Ledger (ANSI B)", 11, 17),
                    ("Tabloid (ANSI B)", 17, 11),
                    ("ANSI C", 22, 17),
                    ("ANSI D", 34, 22),
                    ("ANSI E", 44, 34)
                };

                const int inchesToPoints = 72;

                container.Page(page =>
                {
                    page.Content()
                    .Padding(10)
                    .MinimalBox()
                    .Border(1)
                    .Table(table =>
                    {
                        IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                        {
                            return container
                                .Border(1)
                                .BorderColor(Colors.Grey.Lighten1)
                                .Background(backgroundColor)
                                .PaddingVertical(5)
                                .PaddingHorizontal(10)
                                .AlignCenter()
                                .AlignMiddle();
                        }

                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();

                            columns.ConstantColumn(75);
                            columns.ConstantColumn(75);

                            columns.ConstantColumn(75);
                            columns.ConstantColumn(75);
                        });

                        table.Header(header =>
                        {
                            // please be sure to call the 'header' handler!

                            header.Cell().RowSpan(2).Element(CellStyle).ExtendHorizontal().AlignLeft().Text("Document type");

                            header.Cell().ColumnSpan(2).Element(CellStyle).Text("Inches");
                            header.Cell().ColumnSpan(2).Element(CellStyle).Text("Points");

                            header.Cell().Element(CellStyle).Text("Width");
                            header.Cell().Element(CellStyle).Text("Height");

                            header.Cell().Element(CellStyle).Text("Width");
                            header.Cell().Element(CellStyle).Text("Height");

                            // you can extend existing styles by creating additional methods
                            IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                        });

                        foreach (var page in pageSizes)
                        {
                            table.Cell().Element(CellStyle).ExtendHorizontal().AlignLeft().Text(page.name);

                            // inches
                            table.Cell().Element(CellStyle).Text(page.width);
                            table.Cell().Element(CellStyle).Text(page.height);

                            // points
                            table.Cell().Element(CellStyle).Text(page.width * inchesToPoints);
                            table.Cell().Element(CellStyle).Text(page.height * inchesToPoints);

                            IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.White).ShowOnce();
                        }
                    });
                });
            });
            var filename = $"Text.pdf";
            var path = Path.Combine("C:\\Users\\Developer\\source\\repos\\Web-with-Pdf\\WebApi_with_QuestPDF\\PdfFiles\\", filename);

            document.GeneratePdf(path);

            return Ok("Success !");
            

        }
        [HttpGet("GenerateReportPdfFile")]
        private IActionResult GenerateReportPdfFile()
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(2, Unit.Centimetre);

                    page.Content()
                    .MinimalBox()
                    .Border(1)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(100);
                            columns.RelativeColumn();
                            columns.ConstantColumn(100);
                            columns.RelativeColumn();
                        });

                        table.ExtendLastCellsToTableBottom();

                        table.Cell().RowSpan(3).LabelCell("Project");
                        table.Cell().RowSpan(3).ShowEntire().ValueCell().Text(Placeholders.Sentence());

                        table.Cell().LabelCell("Report number");
                        table.Cell().ValueCell().Text("12");

                        table.Cell().LabelCell("Date");
                        table.Cell().ValueCell().Text(Placeholders.ShortDate());

                        table.Cell().LabelCell("Inspector");
                        table.Cell().ValueCell().Text("Marcin Ziąbek");

                        table.Cell().ColumnSpan(2).LabelCell("Morning weather");
                        table.Cell().ColumnSpan(2).LabelCell("Evening weather");

                        table.Cell().ValueCell().Text("Time");
                        table.Cell().ValueCell().Text("7:13");

                        table.Cell().ValueCell().Text("Time");
                        table.Cell().ValueCell().Text("18:25");

                        table.Cell().ValueCell().Text("Description");
                        table.Cell().ValueCell().Text("Sunny");
                            
                        table.Cell().ValueCell().Text("Description");
                        table.Cell().ValueCell().Text("Windy");
                                 
                        table.Cell().ValueCell().Text("Wind");
                        table.Cell().ValueCell().Text("Mild");
                                  
                        table.Cell().ValueCell().Text("Wind");
                        table.Cell().ValueCell().Text("Strong");
                                    
                        table.Cell().ValueCell().Text("Temperature");
                        table.Cell().ValueCell().Text("17°C");
                                  
                        table.Cell().ValueCell().Text("Temperature");
                        table.Cell().ValueCell().Text("32°C");

                        table.Cell().LabelCell("Remarks");
                        table.Cell().ColumnSpan(3).ValueCell().Text(Placeholders.Paragraph());
                    });
                });
            });

            var filename = $"Text.pdf";
            var path = Path.Combine("C:\\Users\\Developer\\source\\repos\\Web-with-Pdf\\WebApi_with_QuestPDF\\PdfFiles\\", filename);

            document.GeneratePdf(path);

            return Ok("Success !");
        }
        [HttpGet("GeneratePdf")]
        private IActionResult GeneratePdf()
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

   

    static class SimpleExtension
    {
        private static IContainer Cell(this IContainer container, bool dark)
        {
            return container
                .Border(1)
                .Background(dark ? Colors.Grey.Lighten2 : Colors.White)
                .Padding(10);
        }

        // displays only text label
        public static void LabelCell(this IContainer container, string text) => container.Cell(true).Text(text).Medium();

        // allows you to inject any type of content, e.g. image
        public static IContainer ValueCell(this IContainer container) => container.Cell(false);
    }
}
