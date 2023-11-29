namespace Tazor;

public interface IRunnerOptions
{
    string OutputPath { get; }
    
    string AssetPath { get; }
    
    string AppPath { get; }
    
    string BasePath { get; }
}