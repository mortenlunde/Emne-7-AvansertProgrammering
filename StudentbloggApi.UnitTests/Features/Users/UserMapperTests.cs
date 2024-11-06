using StudentBlogg.Common.Interfaces;
using StudentBlogg.Feature.Users;

namespace StudentblogApi.UnitTests.Features.Users;

public class UserMapperTests
{
    private readonly IMapper<User, UserDto> _userMapper = new UserMapper();

    [Fact]
    public void MapToDto_When_UserModelIsValid_ShouldReturn_UserDto()
    {
        // Arrange
        User user = new()
        {
            Id = new Guid(),
            FirstName = "Line",
            LastName = "Lunde",
            Username = "Ariel",
            Email = "Line@lunde.no",
            IsAdminUser = true,
            Created = new DateTime(2024, 10, 30, 7, 45, 00),
            Updated = new DateTime(2024, 10, 30, 9, 45, 00),
            HashedPassword = "g4earfgd"
        };

        // Act
        UserDto userDto = _userMapper.MapToDto(user);

        // Assert
        Assert.NotNull(userDto);
        Assert.Equal(user.Id, userDto.Id);
        Assert.Equal(user.Email, userDto.Email);
        Assert.Equal(user.FirstName, userDto.FirstName);
        Assert.Equal(user.LastName, userDto.LastName);
        Assert.Equal(user.Username, userDto.Username);
        Assert.Equal(user.Created, userDto.Created);
        Assert.Equal(user.Updated, userDto.Updated);
    }

    [Fact]
    public void MapToModel_When_UserModelIsValid_ShouldReturn_UserDto()
    {
        UserDto userdto = new()
        {
            Id = new Guid(),
            FirstName = "Line",
            LastName = "Lunde",
            Username = "Ariel",
            Email = "Line@lunde.no",
            Created = new DateTime(2024, 10, 30, 7, 45, 00),
            Updated = new DateTime(2024, 10, 30, 9, 45, 00),
        };

        // Act
        User user = _userMapper.MapToModel(userdto);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(userdto.Id, user.Id);
        Assert.Equal(userdto.Email, user.Email);
        Assert.Equal(userdto.FirstName, user.FirstName);
        Assert.Equal(userdto.LastName, user.LastName);
        Assert.Equal(userdto.Username, user.Username);
        Assert.Equal(userdto.Created, user.Created);
        Assert.Equal(userdto.Updated, user.Updated);
    }
}