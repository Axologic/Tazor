using System.Reflection;
using TinySitemapGenerator;

namespace Tazor;

public class SitemapProcessor : IDocumentsProcessor
{
    private readonly IRunnerOptions _options;

    public SitemapProcessor(IRunnerOptions options)
    {
        _options = options;
    }

    public async Task Process(IEnumerable<Document> documents)
    {
        var sitemap = new Sitemap
        {
            Filepath = _options.Output
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