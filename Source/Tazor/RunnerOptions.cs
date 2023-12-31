using CommandLine;

namespace Tazor;

public class RunnerOptions : IRunnerOptions
{
    public RunnerOptions(string contentRootPath)
    {
        ContentRootPath = contentRootPath;
        OutputPath = Path.Combine(contentRootPath, "Output");
    }

    [Option('o', "output", Required = false, HelpText = "Set the output path.")]
    public string OutputPath { get; set; }

    [Option('b', "base", Required = false, HelpText = "Set the base path.")]
    public string BasePath { get; set; } = "/";

    [Option('s', "silent", Required = false, HelpText = "Sets whether to preview the content.")]
    public bool Silent { get; set; } = false;
    
    public string AssetPath { get; } = "wwwroot";

    public string ContentRootPath { get; }
}