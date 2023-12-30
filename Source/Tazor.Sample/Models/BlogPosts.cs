using AutoBogus;

namespace Tazor.Sample.Models;

public static class BlogPosts
{
    private static IEnumerable<BlogPost>? _blogPosts;
    
    public static IEnumerable<BlogPost> GetBlogPosts()
    {
        return _blogPosts ??= new AutoFaker<BlogPost>()
            .RuleFor(c => c.Title, f => f.Lorem.Sentence())
            .RuleFor(c => c.PublishedOn, f => f.Date.Recent())
            .RuleFor(c => c.Slug, f=> f.Lorem.Slug())
            .RuleFor(c => c.Content, f => f.Lorem.Paragraphs(5))
            .Generate(100).ToArray();
    }
}