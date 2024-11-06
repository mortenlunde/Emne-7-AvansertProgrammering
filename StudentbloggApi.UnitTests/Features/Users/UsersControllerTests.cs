using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StudentBlogg.Feature.Posts.Interfaces;
using StudentBlogg.Feature.Users;
using StudentBlogg.Feature.Users.Interfaces;

namespace StudentblogApi.UnitTests.Features.Users;

public class UsersControllerTests
{
    private readonly UsersController _usersController;
    private readonly Mock<ILogger<UsersController>> _mockLogger = new();
    private readonly Mock<IUserService> _mockUserService = new();
    private readonly Mock<IPostService> _mockPostService = new();

    public UsersControllerTests()
    {
        _usersController = new UsersController(_mockLogger.Object, _mockUserService.Object, _mockPostService.Object);

    }
    [Fact]
    public async Task GetUsersAsync_WhenDefaultPagesSizeAndOneUserExists_ShouldReturOneUser()
    {
        // Arrage
        string username = "Ariel";
        List<UserDto> userDtos =
        [
            new UserDto()
            {
                Id = Guid.NewGuid(),
                Username = username,
                FirstName = "Line",
                LastName = "Lunde",
                Email = "line@lunde.no",
                Created = DateTime.UtcNow.AddDays(-2),
                Updated = DateTime.UtcNow
            }
        ];
        
        // _userService.GetPagedAsync(pagenr,pagesize)
        _mockUserService.Setup(x => 
                x.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(userDtos);

        // Act
        ActionResult<IEnumerable<UserDto>> result = await _usersController.GetUsersAsync(new UserSearchParams());

        // Assert
        ActionResult<IEnumerable<UserDto>> actionResult = Assert.IsType<ActionResult<IEnumerable<UserDto>>>(result);
        OkObjectResult returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
        List<UserDto> userdtoo = Assert.IsType<List<UserDto>>(returnValue.Value);
        UserDto? dtoo = userdtoo.FirstOrDefault();
        Assert.NotNull(dtoo);
        Assert.Equal(dtoo.Username, username);
    }
}