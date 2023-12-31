using TinySitemapGenerator;

namespace Tazor.Processors;

public class SitemapProcessor : IDocumentsProcessor
{
    private readonly IGeneratorOptions _options;

    public SitemapProcessor(IGeneratorOptions options)
    {
        _options = options;
    }

    public async Task Process(Document[] documents)
    {
        var sitemap = new Sitemap
        {
            Filepath = _options.OutputPath
        };

        foreach (var document in documents)
        {
            var url = new SitemapUrl { 
                Location = document.Url,
                LastModified = DateTime.Now,
                ChangeFrequency = SitemapChangeFrequencies.Daily
            };
            
            sitemap.SitemapUrls.Add(url);
        }

        await sitemap.SaveSitemapAsync();
    }
}