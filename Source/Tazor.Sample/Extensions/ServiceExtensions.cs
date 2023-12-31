using CommandLine;

using Microsoft.Extensions.FileProviders;

using Tazor.Services;

namespace Tazor.Sample.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddTazor(this IServiceCollection services)
    {
        services.AddHostedService<TazorHostedService>();
        services.AddSingleton(sp =>
        {
            var parser = new Parser();
            var environment = sp.GetRequiredService<IHostEnvironment>();
            var options = parser.ParseArguments(() => new RunnerOptions(environment.ContentRootPath), Environment.GetCommandLineArgs()).Value;
            return options;
        });

        return services;
    }

    public static IApplicationBuilder UseTazor(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetRequiredService<RunnerOptions>();

        Directory.CreateDirectory(options.OutputPath);
        
        var fileProvider = new PhysicalFileProvider(options.OutputPath);

        app.Use(async (context, func) =>
        {
            var file = $"{context.Request.Path.Value?.TrimStart('/') ?? "index"}.html";
            var htmlPath = Path.Combine(options.OutputPath, file);
            if (File.Exists(htmlPath))
            {
                var text = await File.ReadAllTextAsync(htmlPath);
                await context.Response.WriteAsync(text);
            }
            else
            {
                await func.Invoke();
            }
        });

        app.UseDefaultFiles(new DefaultFilesOptions
        {
            FileProvider = fileProvider
        });

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = fileProvider
        });

        return app;
    }
    
}