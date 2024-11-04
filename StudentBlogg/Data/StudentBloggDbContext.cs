using Microsoft.EntityFrameworkCore;
using StudentBlogg.Feature.Comments;
using StudentBlogg.Feature.Posts;
using StudentBlogg.Feature.Users;
namespace StudentBlogg.Data;

public class StudentBloggDbContext(DbContextOptions<StudentBloggDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(p => p.Username).IsUnique();
        });
    }
}