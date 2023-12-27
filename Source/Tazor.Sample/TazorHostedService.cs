namespace Tazor.Sample2;

public class TazorHostedService : IHostedService
{
    private readonly IHostEnvironment _environment;

    public TazorHostedService(IHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var output = Path.Combine(_environment.ContentRootPath, "Output");
        var builder = new RunnerBuilder(new RunnerOptions
        {
            OutputPath = output
        });
        
        var runner = builder.Build();

        await runner.Run();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}