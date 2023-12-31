using Tazor.Processors;
using Tazor.Resolvers;

namespace Tazor;

public class Generator : IGenerator
{
    private readonly IEnumerable<IDocumentResolver> _resolvers;
    private readonly IEnumerable<IDocumentsProcessor> _processors;
    private readonly IGeneratorOptions _options;

    public Generator(IEnumerable<IDocumentResolver> resolvers, IEnumerable<IDocumentsProcessor> processors, IGeneratorOptions options)
    {
        _resolvers = resolvers;
        _processors = processors;
        _options = options;
    }

    public async Task Run()
    {
        Directory.CreateDirectory(_options.OutputPath);
        
        var documents = _resolvers
            .Select(async r => await r.GetDocuments())
            .SelectMany(t => t.Result)
            .ToArray();
        
        Console.WriteLine($"Resolved {documents.Length} documents:");
        foreach (var document in documents)
        {
            Console.WriteLine($"  {document.Url}");
        }
        Console.WriteLine();

        foreach (var processor in _processors)
        {
            await processor.Process(documents);
        }

        var assets = Directory.GetFiles(Path.Combine(_options.ContentRootPath, _options.AssetPath), "*.*", SearchOption.AllDirectories);
        
        Console.WriteLine();
        Console.WriteLine($"Found {assets.Length} assets:");
        
        foreach (var asset in assets)
        {
            var relativePath = Path.GetRelativePath(Path.Combine(_options.ContentRootPath, _options.AssetPath), asset);
            File.Copy(asset, Path.Combine(_options.OutputPath, relativePath), true);
            
            Console.WriteLine($"  {relativePath}");
        }
    }
}