using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudentBlogg.Feature.Comments;
using StudentBlogg.Feature.Users;

namespace StudentBlogg.Feature.Posts;

public class Post
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("UserID")]
    public Guid UserId { get; set; }
    
    [Required, MinLength(2), MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    public DateTime DatePosted { get; set; }
    
    public virtual User? User1 { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}