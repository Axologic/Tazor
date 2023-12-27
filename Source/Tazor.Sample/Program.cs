using Tazor.Sample2;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<TazorHostedService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
    DefaultContentType = "text/plain"
});

app.Run();