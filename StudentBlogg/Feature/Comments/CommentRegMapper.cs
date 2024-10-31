using StudentBlogg.Common.Interfaces;

namespace StudentBlogg.Feature.Comments;

public class CommentRegMapper : IMapper<Comment, CommentRegDto>
{
    public CommentRegDto MapToDto(Comment model)
    {
        return new CommentRegDto()
        {
            PostId = model.PostId,
            UserId = model.UserId,
            Content = model.Content
        };
    }

    public Comment MapToModel(CommentRegDto dto)
    {
        return new Comment()
        {
            PostId = dto.PostId,
            UserId = dto.UserId,
            Content = dto.Content
        };
    }
}