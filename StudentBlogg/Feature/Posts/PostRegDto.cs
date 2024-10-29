namespace StudentBlogg.Feature.Posts;

public class PostRegDto
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}