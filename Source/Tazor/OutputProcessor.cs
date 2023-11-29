﻿using System.Reflection;

namespace Tazor;

public class OutputProcessor : IDocumentsProcessor
{
    private readonly IRunnerOptions _options;

    public OutputProcessor(IRunnerOptions options)
    {
        _options = options;
    }

    public async Task Process(Document[] documents)
    {
        foreach (var document in documents)
        {
            var outputPath = Path.Combine(_options.OutputPath, $"{Sanitize(document.Url)}.html");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        
            await File.WriteAllTextAsync(outputPath, document.Html);
        }
        
        Console.WriteLine($"Outputted {documents.Length} documents to {_options.OutputPath}");
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