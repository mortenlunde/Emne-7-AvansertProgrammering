using Microsoft.AspNetCore.Mvc;
using StudentBlogg.Feature.Comments.Interfaces;

namespace StudentBlogg.Feature.Comments;

[ApiController]
[Route("api/v1/[controller]")]
public class CommentController(ILogger<CommentController> logger, ICommentService commentService) : ControllerBase
{
    private readonly ILogger<CommentController> _logger = logger;
    private readonly ICommentService _commentService = commentService;

    [HttpGet("{id:guid}", Name = "GetComment")]
    public async Task<ActionResult<CommentDto>> GetComment(Guid id)
    {
        var commentDto = await _commentService.GetByIdAsync(id);
        if (commentDto == null)
            _logger.LogError($"Comment with id {id} not found");
        
        return commentDto is null 
            ? NotFound("Comment not found.") 
            : Ok(commentDto);
    }

    [HttpGet(Name = "GetComments")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(int page = 1, int pageSize = 10)
    {
        var commentDtos = await _commentService.GetPagedAsync(page, pageSize);
        return Ok(commentDtos);
    }

    [HttpPost("Post", Name = "PostComment")]
    public async Task<ActionResult<CommentDto>> PostComment(CommentDto dto)
    {
        var comment = await _commentService.AddAsync(dto);

        return comment is null
            ? BadRequest("Comment not created")
            : Ok(comment);
    }
}