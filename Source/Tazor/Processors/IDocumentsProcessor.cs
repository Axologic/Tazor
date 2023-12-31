namespace Tazor.Processors;

public interface IDocumentsProcessor
{
    Task Process(Document[] documents);
}