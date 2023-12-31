namespace Tazor.Resolvers;

public interface IDocumentResolver
{
    Task<Document[]> GetDocuments();
}