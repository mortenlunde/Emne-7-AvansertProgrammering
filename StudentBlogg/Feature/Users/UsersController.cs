using Microsoft.AspNetCore.Mvc;
using StudentBlogg.Feature.Users.Interfaces;

namespace StudentBlogg.Feature.Users;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(ILogger<UsersController> logger, IUserService userService) : ControllerBase
{
    private readonly ILogger<UsersController> _logger = logger;
    private readonly IUserService _userService = userService;
    
    [HttpGet("{id:guid}", Name = "GetUserByIDAsync")]
    public async Task<ActionResult<UserDto>> GetUserByIdAsync(Guid id)
    {
        var userDto = await _userService.GetByIdAsync(id);
        if (userDto == null)
            _logger.LogError($"User with ID {id} not found");
        return userDto is null 
            ? NotFound("User not found") 
            : Ok(userDto);
    }
    
    [HttpGet(Name = "GetUsersAsync")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersAsync(
        [FromQuery] UserSearchParams? searchParams, [FromQuery] int pageNr = 1, [FromQuery] int pageSize = 10)
    {
        if (searchParams!.Firstname is null && searchParams.Lastname is null 
                                            && searchParams.Email is null 
                                            && searchParams.Username is null)
        {
            IEnumerable<UserDto> userDtos = await _userService.GetPagedAsync(pageNr, pageSize);
            return Ok(userDtos);
        }

        return Ok(await _userService.FindAsync(searchParams));
    }

    [HttpPost("Register", Name = "RegisterUserAsync")]
    public async Task<ActionResult<UserDto>> RegisterUserAsync(UserRegistrationDto dto)
    {
        var user = await _userService.RegisterAsync(dto);
        return user is null
            ? BadRequest("Failed to register new user")
            : Ok(user);
    }

    [HttpDelete("{id:guid}", Name = "DeleteUserAsync")]
    public async Task<ActionResult<UserDto>> DeleteUserAsync(Guid id)
    {
        _logger.LogInformation($"User with ID {id} deleted");
        var result = await _userService.DeleteByIdAsync(id);
        
        return result is null
            ? BadRequest("Failed to delete user")
            :Ok(result);
    }
}