using Tazor;

var builder = new RunnerBuilder(args);
var runner = builder.Build();

await runner.Run();