using System.Reflection;
using TinySitemapGenerator;

namespace Tazor;

public class SitemapProcessor : IDocumentsProcessor
{
    public async Task Process(IEnumerable<Document> documents)
    {
        var sitemap = new Sitemap
        {
            Filepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Output")
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