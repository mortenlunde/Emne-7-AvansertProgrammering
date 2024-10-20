using System.Linq.Expressions;
using StudentBlogg.Common.Interfaces;
using StudentBlogg.Feature.Users.Interfaces;
namespace StudentBlogg.Feature.Users;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper<User, UserDto> _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IMapper<User, UserRegistrationDto> _userRegistrationMapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(ILogger<UserService> logger, IMapper<User, UserDto> mapper, IUserRepository userRepository, IMapper<User, UserRegistrationDto> userRegistrationMapper, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _mapper = mapper;
        _userRepository = userRepository;
        _userRegistrationMapper = userRegistrationMapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        User? model = await _userRepository.GetByIdAsync(id);
        return model is null
            ? null
            : _mapper.MapToDto(model);
    }

    public async Task<IEnumerable<UserDto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        IEnumerable<User> users = await _userRepository.GetPagedAsync(pageNumber, pageSize);
        
        return users.Select(usr => _mapper.MapToDto(usr)).ToList();
    }
    
    public async Task<UserDto?> AddAsync(UserDto dto)
    {
        User model = _mapper.MapToModel(dto);
        User? modelResponse = await _userRepository.AddAsync(model);
        
        return modelResponse is null 
            ? null 
            : _mapper.MapToDto(modelResponse);
    }

    public async Task<UserDto?> UpdateAsync(UserDto entity)
    {
        await Task.Delay(10);
        return null;
    }

    public async Task<UserDto?> DeleteByIdAsync(Guid id)
    {
        var loggedInUserId = _httpContextAccessor.HttpContext?.Items["UserId"] as string;
        var loggedInUser = (await _userRepository.FindAsync(user => user.Id.ToString() == loggedInUserId)).FirstOrDefault();

        if (loggedInUser is null)
        {
            _logger.LogWarning("User with ID {UserID} not found", loggedInUserId);
            return null;
        }

        if (id.ToString().Equals(loggedInUser.Id.ToString()) || loggedInUser.IsAdminUser)
        {
            _logger.LogDebug("Deleting user with ID: {UserID}", loggedInUserId);
            var deletedUser = await _userRepository.DeleteByIdAsync(id);

            if (deletedUser == null)
            {
                _logger.LogWarning("User with ID {UserID} not found", id);
                return null;
            }
            
            return _mapper.MapToDto(deletedUser);
        }
        return null;
    }

    public async Task<UserDto> RegisterAsync(UserRegistrationDto regDto)
    {
        User user = _userRegistrationMapper.MapToModel(regDto);
        user.Created = DateTime.UtcNow;
        user.Updated = DateTime.UtcNow;
        user.IsAdminUser = false;
        user.Id = Guid.NewGuid();
        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(regDto.Password);
        
        User? userResponse = await _userRepository.AddAsync(user);
        return userResponse is null
            ? null
            : _mapper.MapToDto(userResponse);
    }

    public async Task<Guid> AuthenticateUserAsync(string username, string password)
    {
        Expression<Func<User, bool>> expression = user => user.Username == username;
        var usr = (await _userRepository.FindAsync(expression)).FirstOrDefault();
        
        if (usr is null)
            return Guid.Empty;

        if (!BCrypt.Net.BCrypt.Verify(password, usr.HashedPassword))
            return usr.Id;
        
        return Guid.Empty;
    }

    public async Task<IEnumerable<UserDto>> FindAsync(UserSearchParams searchParams)
    {
        Expression<Func<User, bool>> predicate = user =>
            (string.IsNullOrEmpty(searchParams.Username) || user.Username.Contains(searchParams.Username)) &&
            (string.IsNullOrEmpty(searchParams.Firstname) || user.FirstName.Contains(searchParams.Firstname)) &&
            (string.IsNullOrEmpty(searchParams.Lastname) || user.LastName.Contains(searchParams.Lastname)) &&
            (string.IsNullOrEmpty(searchParams.Email) || user.Email.Contains(searchParams.Email));
        
        var users = await _userRepository.FindAsync(predicate);
        
        return users.Select(u => _mapper.MapToDto(u));
    }
}