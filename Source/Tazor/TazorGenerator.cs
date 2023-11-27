namespace Tazor;

public static class TazorGenerator
{
    public static async Task Generate()
    {
        var resolvers = new IDocumentResolver[]
        {
            new RazorComponentsResolver()
        };
        
        var processors = new IDocumentsProcessor[]
        {
            new OutputProcessor(),
            new SitemapProcessor()
        };
        
        var documents = resolvers
            .Select(async r => await r.GetDocuments())
            .SelectMany(t => t.Result)
            .ToArray();
        
        Console.WriteLine($"Resolved {documents.Length} documents:");
        foreach (var document in documents)
        {
            Console.WriteLine($"  {document.Url}");
        }

        foreach (var processor in processors)
        {
            await processor.Process(documents);
        }
    }
}