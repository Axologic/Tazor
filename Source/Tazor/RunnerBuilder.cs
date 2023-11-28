using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Tazor;

public class RunnerBuilder
{
    private readonly RunnerOptions _options;
    private readonly ServiceCollection _services = new();

    public RunnerBuilder(IEnumerable<string> args)
    {
        _options = Parser.Default.ParseArguments<RunnerOptions>(args).Value;
        _services.AddTransient<IRunnerOptions>(_ => _options);
        _services.AddTransient<Runner>();
        _services.AddTransient<IDocumentResolver, RazorComponentsResolver>();
        _services.AddTransient<IDocumentsProcessor, OutputProcessor>();
        _services.AddTransient<IDocumentsProcessor, SitemapProcessor>();
    }

    public RunnerBuilder WithOutput(string outputPath)
    {
        _options.Output = outputPath;
        return this;
    }

    public RunnerBuilder WithServices(Action<ServiceCollection> withServices)
    {
        withServices.Invoke(_services);
        return this;
    }

    public Runner Build()
    {
        var provider = _services.BuildServiceProvider();
        return provider.GetService<Runner>()!;
    }
}