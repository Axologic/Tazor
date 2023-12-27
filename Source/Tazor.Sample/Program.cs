using Microsoft.Extensions.FileProviders;
using Tazor.Sample2;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<TazorHostedService>();

var app = builder.Build();

var outputPath = Path.Combine(builder.Environment.ContentRootPath, "Output");
Directory.CreateDirectory(outputPath);
var fileProvider = new PhysicalFileProvider(outputPath);

app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = fileProvider
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = fileProvider
});


app.Run();