using Microsoft.AspNetCore.Mvc;
using StudentBlogg.Feature.Posts.Interfaces;

namespace StudentBlogg.Feature.Posts;

[ApiController]
[Route("api/v1/[controller]")]
public class PostController(ILogger<PostController> logger, IPostService postService) : ControllerBase
{
    private readonly ILogger<PostController> _logger = logger;
    private readonly IPostService _postService = postService;

    [HttpGet("{id:guid}", Name = "GetPost")]
    public async Task<ActionResult<PostDto>> GetPostAsync(Guid id)
    {
        var postDto = await _postService.GetByIdAsync(id);
        if (postDto == null)
            _logger.LogError($"Post with id {id} not found.");
        
        return postDto is null
            ? NotFound("Post not found.")
            : Ok(postDto);
    }

    [HttpGet(Name = "GetPosts")]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts(int page = 1, int pageSize = 10)
    {
        var postDtos = await _postService.GetPagedAsync(page, pageSize);
        
        return Ok(postDtos);
    }

    [HttpPost("Post", Name = "CreatePost")]
    public async Task<ActionResult<PostDto>> CreatePost(PostRegDto dto)
    {
        var post = await _postService.CreatePostAsync(dto);

        return post is null
            ? BadRequest("Post not created")
            : Ok(post);
    }

    [HttpDelete("{id:guid}", Name = "DeletePost")]
    public async Task<ActionResult<PostDto>> DeletePost(Guid id)
    {
        _logger.LogInformation($"Post with id {id} deleted.");
        var result = await _postService.DeleteByIdAsync(id);
        
        return result is null
            ? BadRequest("Failed to delete post.")
            : Ok(result);
    }

    [HttpPut("{id:guid}", Name = "UpdatePost")]
    public async Task<ActionResult<PostDto>> UpdatePost(Guid id, PostDto dto)
    {
        _logger.LogInformation($"Attempting to update post with id {id}.");
        var result = await postService.UpdateAsync(id, dto);

        return result is null
            ? BadRequest("Failed to update post.")
            : Ok(result);
    }

}