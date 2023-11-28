using System.Reflection;

namespace Tazor;

public class OutputProcessor : IDocumentsProcessor
{
    private readonly IRunnerOptions _options;

    public OutputProcessor(IRunnerOptions options)
    {
        _options = options;
    }

    public async Task Process(IEnumerable<Document> documents)
    {
        foreach (var document in documents)
        {
            var outputPath = Path.Combine(_options.Output, $"{Sanitize(document.Url)}.html");
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