using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StudentBlogg.Feature.Users;
using StudentBlogg.Feature.Users.Interfaces;

namespace StudentblogApi.UnitTests.Features.Users;

public class UsersControllerTests
{
    private readonly UsersController _usersController;
    private readonly Mock<ILogger<UsersController>> _mockLogger = new();
    private readonly Mock<IUserService> _mockUserService = new();

    public UsersControllerTests()
    {
        _usersController = new UsersController(_mockLogger.Object, _mockUserService.Object);

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
        var result = await _usersController.GetUsersAsync(new UserSearchParams());

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<UserDto>>>(result);
        var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
        var userdtoo = Assert.IsType<List<UserDto>>(returnValue.Value);
        var dtoo = userdtoo.FirstOrDefault();
        Assert.NotNull(dtoo);
        Assert.Equal(dtoo.Username, username);
    }
}