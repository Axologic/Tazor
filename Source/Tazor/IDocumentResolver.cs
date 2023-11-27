namespace Tazor;

public interface IDocumentResolver
{
    Task<IEnumerable<Document>> GetDocuments();
}