namespace Tazor;

public interface IDocumentResolver
{
    Task<Document[]> GetDocuments();
}