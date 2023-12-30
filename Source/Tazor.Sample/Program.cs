using Microsoft.Extensions.FileProviders;

using Tazor.Sample;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<TazorHostedService>();

var app = builder.Build();

var outputPath = Path.Combine(builder.Environment.ContentRootPath, "Output");
Directory.CreateDirectory(outputPath);
var fileProvider = new PhysicalFileProvider(outputPath);

app.Use(async (context, func) =>
{
    var file = $"{context.Request.Path.Value?.TrimStart('/') ?? "index"}.html";
    var htmlPath = Path.Combine(outputPath, file);
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


app.Run();