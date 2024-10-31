namespace StudentBlogg.Feature.Comments.Interfaces;

public interface ICommentMapper
{
    CommentDto MapToDto(Comment comment);
    Comment MapToDto(CommentDto commentDto);
}