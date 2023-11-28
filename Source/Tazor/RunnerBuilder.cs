using Microsoft.Extensions.DependencyInjection;

namespace Tazor;

public class RunnerBuilder
{
    private readonly RunnerOptions _options = new();
    private readonly ServiceCollection _services = new();

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
        return new Runner(_services.BuildServiceProvider(), _options);
    }
}