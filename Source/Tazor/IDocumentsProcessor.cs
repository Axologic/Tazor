namespace Tazor;

public interface IDocumentsProcessor
{
    Task Process(Document[] documents);
}