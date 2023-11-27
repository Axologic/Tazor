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

        foreach (var processor in processors)
        {
            await processor.Process(documents);
        }
    }
}