using System.Reflection;

namespace Tazor;

public class RunnerOptions
{
    public string Output { get; set; } = Path.Combine(Assembly.GetEntryAssembly()!.Location, "Output");
}