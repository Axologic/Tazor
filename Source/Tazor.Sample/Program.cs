using Tazor.Sample.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTazor();

var app = builder.Build();
app.UseTazor();

await app.RunAsync();
