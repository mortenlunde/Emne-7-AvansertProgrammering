using StudentBlogg.Common.Interfaces;
using StudentBlogg.Feature.Comments.Interfaces;
using StudentBlogg.Feature.Posts;
using StudentBlogg.Feature.Posts.Interfaces;
using StudentBlogg.Feature.Users;
using StudentBlogg.Feature.Users.Interfaces;
using StudentBlogg.Middleware;

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
        string loggedInUserId = _httpContextAccessor.HttpContext.Items["UserId"] as string;
        
        if (!Guid.TryParse(loggedInUserId, out Guid userId))
        {
            _logger.LogWarning("Invalid user id format: {UserId}", userId);
            return null;
        }
        
        User loggedInUser = (await _userRepository.FindAsync(user => user.Id == userId)).FirstOrDefault();
        if (loggedInUser is null)
        {
            _logger.LogWarning("User id not found: {UserId}", userId);
        }
        
        Comment commentToUpdate = (await _commentRepository.FindAsync(c => c.Id == id)).FirstOrDefault();
        if (commentToUpdate is null)
        {
            _logger.LogWarning("Comment not found: {Id}", id);
            return null;
        }

        if (commentToUpdate.UserId == loggedInUser.Id || loggedInUser.IsAdminUser)
        {
            commentToUpdate.Content = entity.Content;
            Comment? updatedComment = await _commentRepository.UpdateByIdAsync(id, commentToUpdate);
            
            return updatedComment is null
                ? null
                : _mapper.MapToDto(updatedComment);
        }
        else
        {
            throw new WrongUserLoggedInException();
        }
    }

    public async Task<CommentDto?> DeleteByIdAsync(Guid id)
    {
        string loggedInUserId = _httpContextAccessor.HttpContext.Items["UserId"] as string;
        if (!Guid.TryParse(loggedInUserId, out Guid loggedInUserGuid))
        {
            _logger.LogWarning("Invalid UserId format: {UserId}", loggedInUserId);
            return null;
        }
        
        User? loggedInUser = (await _userRepository.FindAsync(user => user.Id == loggedInUserGuid)).FirstOrDefault();
        if (loggedInUser == null)
        {
            _logger.LogWarning("User {UserId} not found", loggedInUserId);
            return null;
        }
        
        IEnumerable<Comment> comments = await _commentRepository.FindAsync(c => c.Id == id);
        Comment? commentToDelete = comments.FirstOrDefault();

        if (commentToDelete is null)
        {
            _logger.LogWarning("Comment not found: {Id}", id);
            return null;
        }

        if (commentToDelete.UserId == loggedInUser.Id || loggedInUser.IsAdminUser)
        {
            _logger.LogInformation("Deleting comment with id {commentID} for user {userID}", id, loggedInUserId);
            Comment? deletedComment = await _commentRepository.DeleteByIdAsync(id);

            if (deletedComment is null)
            {
                _logger.LogWarning("Comment with id {commentID} not found", id);
                return null;
            }
            
            return _mapper.MapToDto(deletedComment);
        }
        _logger.LogWarning("User {UserId} not autorized to delete comment with id {commentId}", loggedInUserId, id);
        return null;
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