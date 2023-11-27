namespace Tazor.Sample.Models;

public class BlogPost
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = null!;

    public DateTime PublishedOn { get; set; }
}