namespace Tazor.Sample.Models;

public class Page<T>
{
    public int Number { get; set; }
    
    public int Size { get; set; }
    
    public int Total { get; set; }

    public T[] Items { get; set; } = Array.Empty<T>();
}