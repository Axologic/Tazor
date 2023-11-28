using System.Reflection;
using CommandLine;

namespace Tazor;

public interface IRunnerOptions
{
    string Output { get; }
}

public class RunnerOptions : IRunnerOptions
{
    [Option('o', "output", Required = false, HelpText = "Set the output path.")]
    public string Output { get; set; } = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!, "Output");
}