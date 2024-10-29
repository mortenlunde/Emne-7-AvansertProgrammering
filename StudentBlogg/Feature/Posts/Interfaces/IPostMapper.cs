namespace StudentBlogg.Feature.Posts.Interfaces;

public interface IPostMapper
{
    PostDto MapToPostDto(Post model);
    Post MapToModel(PostDto postDto);
}