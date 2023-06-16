using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using WebApi_with_QuestPDF.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var doc = Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(16));


                page.Header().Element(Header);

                page.Content().Element(Content);

                page.Footer().AlignCenter().Text($" Душанбе {DateTime.Now.ToString("yyyy")}").FontFamily("Arial").Black();
            });
        });
        QuestPDF.Settings.License = LicenseType.Community;
        // doc.ShowInPreviewer();
        var filename = $"product.{Guid.NewGuid().ToString()}.pdf";
        var path =Path.Combine("C:\\Users\\Developer\\source\\repos\\Web-with-Pdf\\ConsoleApp_Pdf\\PdfFiles\\",filename);
        doc.GeneratePdf(path);

        void Header(IContainer container)
        {
            container.Row(row =>
            {
                row.Spacing(25);

                row.ConstantItem(100).Image("C:\\Users\\Developer\\source\\repos\\Web-with-Pdf\\ConsoleApp_Pdf\\Images\\SoftClub.Logo.jpg");

                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("SoftClub Academy")
                    .FontSize(36)
                    .FontColor(Colors.Orange.Medium)
                    .SemiBold();
                });
            });
        }
        void Content(IContainer container)
        {
            container.PaddingVertical(25).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    //product name,qty,price per item,total price

                    columns.ConstantColumn(50);
                    columns.RelativeColumn();
                    columns.ConstantColumn(75);
                    columns.ConstantColumn(90);
                    columns.ConstantColumn(90); 

                });
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Product Name");
                    header.Cell().Element(CellStyle).AlignRight().Text("Quantity");
                    header.Cell().Element(CellStyle).AlignRight().Text("Price");
                    header.Cell().Element(CellStyle).AlignRight().Text("Total");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });
                foreach (var i in Enumerable.Range(1,10))
                {
                    var qty = 5;
                    var price=10*5;
                    var total = qty * price;



                    table.Cell().Element(CellStyle).Text(i);
                    table.Cell().Element(CellStyle).Text("Самсунг 8 A").FontFamily("Arial");
                    table.Cell().Element(CellStyle).Background(Colors.Blue.Lighten2).AlignCenter().Text(qty);
                    table.Cell().Element(CellStyle).AlignRight().Text(price);
                    table.Cell().Element(CellStyle).AlignRight().Text(total);

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                }
            });
        }
    }
}