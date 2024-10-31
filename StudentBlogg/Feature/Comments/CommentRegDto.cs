namespace StudentBlogg.Feature.Comments;

public class CommentRegDto
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime DateCommented { get; set; }
}