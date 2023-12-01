using CommandLine;

namespace Tazor;

public class RunnerOptions : IRunnerOptions
{
    public RunnerOptions()
    {
        OutputPath = Path.Combine(AppPath, "Output");
    }

    [Option('o', "output", Required = false, HelpText = "Set the output path.")]
    public string OutputPath { get; set; }

    [Option('b', "base", Required = false, HelpText = "Set the base path.")]
    public string BasePath { get; set; } = "/";
    
    public string AssetPath { get; set; } = "wwwroot";

    public string AppPath => Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..");
}