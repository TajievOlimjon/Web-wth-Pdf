using QuestPDF.Infrastructure;

namespace WebApi_with_QuestPDF.Models
{
    public interface IDocument
    {
        void ComposeTable(IContainer container);
    }
}
