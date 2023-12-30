namespace Tazor.Sample.Models;

public class BlogPost
{
    public Guid Id { get; set; }

    public string Slug { get; set; } = null!;
    
    public string Title { get; set; } = null!;

    public DateTime PublishedOn { get; set; }

    public string Content { get; set; } = null!;
}