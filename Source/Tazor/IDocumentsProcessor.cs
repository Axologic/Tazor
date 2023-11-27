namespace Tazor;

public interface IDocumentsProcessor
{
    Task Process(IEnumerable<Document> documents);
}