using Microsoft.AspNetCore.Mvc;
using StudentBlogg.Feature.Comments.Interfaces;
using StudentBlogg.Feature.Posts.Interfaces;

namespace StudentBlogg.Feature.Comments;

[ApiController]
[Route("api/v1/[controller]")]
public class CommentController(ILogger<CommentController> logger, ICommentService commentService, IPostService postService) : ControllerBase
{
    private readonly ILogger<CommentController> _logger = logger;
    private readonly ICommentService _commentService = commentService;
    private readonly IPostService _postService = postService;

    [HttpGet("Comments/{postId:guid}/comments", Name = "GetComment")]
    public async Task<ActionResult<CommentDto>> GetComment(Guid postId)
    {
        var postDto = await _postService.GetByIdAsync(postId);
        var commentDto = await _commentService.GetPagedAsync(1,10);
        var result = commentDto.Where(x => x.PostId == postDto!.Id);
        
        if (commentDto == null)
            _logger.LogError($"Comment with id {postId} not found");
        
        return commentDto is null 
            ? NotFound("Comments not found.") 
            : Ok(result);
    }

    [HttpGet(Name = "GetComments")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(int page = 1, int pageSize = 10)
    {
        var commentDtos = await _commentService.GetPagedAsync(page, pageSize);
        return Ok(commentDtos);
    }

    [HttpPost("Post", Name = "PostComment")]
    public async Task<ActionResult<CommentDto>> PostComment(CommentRegDto dto)
    {
        var comment = await _commentService.AddComment(dto);

        return comment is null
            ? BadRequest("Comment not created")
            : Ok(comment);
    }

    [HttpPut("{id:guid}", Name = "UpdateComment")]
    public async Task<ActionResult<CommentDto>> UpdateComment(Guid id, CommentDto dto)
    {
        _logger.LogInformation($"Attempting to Update comment with id {id}");
        var result = await _commentService.UpdateAsync(id, dto);
        
        return result is null
            ? BadRequest("Comment not updated")
            : Ok(result);
    }

    [HttpDelete("{id:guid}", Name = "DeleteComment")]
    public async Task<ActionResult<CommentDto>> DeleteComment(Guid id)
    {
        _logger.LogInformation($"Attempting to Delete comment with id {id}");
        var result = await _commentService.DeleteByIdAsync(id);
        
        return result is null
            ? BadRequest("Comment not deleted")
            : Ok(result);
    }
}