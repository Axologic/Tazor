using System.Diagnostics;

using CommandLine;

namespace Tazor.Sample;

public class TazorHostedService : IHostedService
{
    private readonly RunnerOptions _options;
    private readonly IHostApplicationLifetime _lifetime;

    public TazorHostedService(RunnerOptions options, IHostApplicationLifetime lifetime)
    {
        _options = options;
        _lifetime = lifetime;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var builder = new RunnerBuilder(_options);
        var runner = builder.Build();

        await runner.Run();

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