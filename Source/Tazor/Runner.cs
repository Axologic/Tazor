using System.Collections;
using CommandLine;

namespace Tazor;

public class Runner
{
    private readonly IEnumerable<IDocumentResolver> _resolvers;
    private readonly IEnumerable<IDocumentsProcessor> _processors;
    private readonly IRunnerOptions _options;

    public Runner(IEnumerable<IDocumentResolver> resolvers, IEnumerable<IDocumentsProcessor> processors, IRunnerOptions options)
    {
        _resolvers = resolvers;
        _processors = processors;
        _options = options;
    }

    public async Task Run()
    {
        var documents = _resolvers
            .Select(async r => await r.GetDocuments())
            .SelectMany(t => t.Result)
            .ToArray();
        
        Console.WriteLine($"Resolved {documents.Length} documents:");
        foreach (var document in documents)
        {
            Console.WriteLine($"  {document.Url}");
        }

        foreach (var processor in _processors)
        {
            await processor.Process(documents);
        }
    }
}