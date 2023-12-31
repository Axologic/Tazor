using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tazor.Services;

public class TazorHostedService : IHostedService
{
    private readonly IGeneratorOptions _options;
    private readonly IGenerator _generator;

    public TazorHostedService(IGeneratorOptions options, IGenerator generator)
    {
        _options = options;
        _generator = generator;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _generator.Run();

        if (_options.Silent)
        {
           Environment.Exit(0);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}