using StudentBlogg.Common.Interfaces;
using StudentBlogg.Feature.Comments.Interfaces;
using StudentBlogg.Feature.Posts.Interfaces;
using StudentBlogg.Feature.Users.Interfaces;

namespace StudentBlogg.Feature.Comments;

public class CommentService : ICommentService
{
    private readonly ILogger<CommentService> _logger;
    private readonly IMapper<Comment, CommentDto> _mapper;
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper<Comment, CommentRegDto> _regMapper;
    private readonly IPostRepository _postRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommentService(ILogger<CommentService> logger, IMapper<Comment, CommentDto> mapper, 
        ICommentRepository commentRepository, IUserRepository userRepository, IMapper<Comment, CommentRegDto> commentMapper,
        IPostRepository postRepository, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _mapper = mapper;
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _regMapper = commentMapper;
        _postRepository = postRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<CommentDto?> GetByIdAsync(Guid id)
    {
        Comment comment = await _commentRepository.GetByIdAsync(id);
        
        return comment is null
            ? null
            : _mapper.MapToDto(comment);
    }

    public async Task<IEnumerable<CommentDto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        IEnumerable<Comment> comments = await _commentRepository.GetPagedAsync(pageNumber, pageSize);
        return comments.Select(comment => _mapper.MapToDto(comment)).ToList();
    }

    public async Task<CommentDto?> AddAsync(CommentDto entity)
    {
        Comment commentModel = _mapper.MapToModel(entity);
        Comment? commentResponse = await _commentRepository.AddAsync(commentModel);
        
        return commentResponse is null
            ? null
            : _mapper.MapToDto(commentResponse);
    }

    public async Task<CommentDto?> UpdateAsync(Guid id, CommentDto entity)
    {
        throw new NotImplementedException();
    }

    public async Task<CommentDto?> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<CommentDto?> AddComment(CommentRegDto regDto)
    {
        Comment comment = _regMapper.MapToModel(regDto);
        comment.Id = Guid.NewGuid();
        if (_httpContextAccessor.HttpContext.Items["UserId"] is string userIdStr 
            && Guid.TryParse(userIdStr, out Guid userId))
        {
            comment.UserId = userId;
        }
        else
        {
            _logger.LogWarning("Invalid or missing UserId in HttpContext.");
            return null;  // or handle the error as appropriate
        }

        if (_httpContextAccessor.HttpContext.Items["PostId"] is string postIdStr 
            && Guid.TryParse(postIdStr, out Guid postId))
        {
            comment.PostId = postId;
        }
        else
        {
            _logger.LogWarning("Invalid or missing PostId in HttpContext.");
            return null;  // or handle the error as appropriate
        }
        comment.DateCommented = DateTime.UtcNow;
        
        Comment commentResponse = await _commentRepository.AddAsync(comment);
        return commentResponse is null
            ? null
            : _mapper.MapToDto(commentResponse);
    }
}