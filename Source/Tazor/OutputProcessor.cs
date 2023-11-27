using System.Reflection;

namespace Tazor;

public class OutputProcessor : IDocumentsProcessor
{
    public async Task Process(IEnumerable<Document> documents)
    {
        var directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Output");

        foreach (var document in documents)
        {
            var outputPath = Path.Combine(directory, $"{Sanitize(document.Url)}.html");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        
            await File.WriteAllTextAsync(outputPath, document.Html);
        }
    }

    private string Sanitize(string url)
    {
        url = url.TrimStart('/').ToLower();
        if (string.IsNullOrWhiteSpace(url))
        {
            url = "index";
        }
        
        return url;
    }
}