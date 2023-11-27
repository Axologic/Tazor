namespace Tazor;

public static class TazorGenerator
{
    public static async Task Generate()
    {
        var resolver = new RazorComponentsResolver();
        var processors = new IDocumentsProcessor[]
        {
            new OutputProcessor(),
            new SitemapProcessor()
        };
        
        var documents = await resolver.GetDocuments();

        foreach (var processor in processors)
        {
            await processor.Process(documents);
        }
    }
}