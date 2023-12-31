namespace Tazor;

public interface IGeneratorOptions
{
    string OutputPath { get; }
    
    string AssetPath { get; }
    
    string ContentRootPath { get; }
    
    string BasePath { get; }
    
    bool Silent { get; }
}