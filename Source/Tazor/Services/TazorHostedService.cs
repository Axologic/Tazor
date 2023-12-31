using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tazor.Services;

public class TazorHostedService : IHostedService
{
    private readonly IRunnerOptions _options;
    private readonly IRunner _runner;

    public TazorHostedService(IRunnerOptions options, IRunner runner)
    {
        _options = options;
        _runner = runner;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _runner.Run();

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