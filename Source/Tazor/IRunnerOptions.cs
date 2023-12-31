namespace Tazor;

public interface IRunnerOptions
{
    string OutputPath { get; }
    
    string AssetPath { get; }
    
    string ContentRootPath { get; }
    
    string BasePath { get; }
}