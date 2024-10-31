using System.Linq.Expressions;
using System.Net;
using Moq;
using Newtonsoft.Json;
using StudentBlogg.Feature.Users;

namespace StudentBloggApi.IntegrationTests.Features.Users;

public class UsersIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    public UsersIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }


    [Fact]
    public async Task GetUsers_WhenNoSearchParams_ThenReturnPagedUsers()
    {
        // Arrange
        List<User> users =
        [
            new()
            {
                Id = Guid.Parse("309e0c57-bf66-4a42-937c-ed59623a067d"),
                FirstName = "string",
                LastName = "string",
                Username = "ymsnew",
                Email = "yms@lunde.no",
                HashedPassword = "$2a$11$CX/gPeaRyux8OIJiV1e2/OEIyCyf8ZgSMpKYeXWgmJ2v9fhme3UGy",
                Created = DateTime.UtcNow.AddDays(-2),
                Updated = DateTime.UtcNow.AddDays(-1),
                IsAdminUser = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Jan Roger",
                LastName = "Lunde",
                Username = "jrl",
                Email = "jrl@lunde.no",
                Created = DateTime.UtcNow.AddDays(-20),
                Updated = DateTime.UtcNow.AddDays(-10),
                HashedPassword = "fsdyri7cksf3ra",
                IsAdminUser = false
            }
        ];
        
        // Authorization: Basic eW1zbmV3OnN0cmluZw==
        _client.DefaultRequestHeaders.Add("Authorization", "Basic eW1zbmV3OnN0cmluZw==");
        
        _factory.UserRepositoryMock.Setup(x => 
            x.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new List<User>(){users[0]});

        _factory.UserRepositoryMock.Setup(z =>
                z.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(users);
        
        // Act
        var response = await _client.GetAsync("/api/v1/Users");
        
        // Assert
        
        var userDtos = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(await response.Content.ReadAsStringAsync());

        Assert.NotNull(userDtos);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Collection(userDtos, user =>
            {
                Assert.Equal(users[0].Email, user.Email);
                Assert.Equal(users[0].Id, user.Id);
                Assert.Equal(users[0].FirstName, user.FirstName);
                Assert.Equal(users[0].LastName, user.LastName);
                Assert.Equal(users[0].Username, user.Username);
                Assert.Equal(users[0].Created, user.Created);
                Assert.Equal(users[0].Updated, user.Updated);
            },
            u =>
            {
                Assert.Equal(users[1].Email, u.Email);
                Assert.Equal(users[1].Id, u.Id);
                Assert.Equal(users[1].FirstName, u.FirstName);
                Assert.Equal(users[1].LastName, u.LastName);
                Assert.Equal(users[1].Username, u.Username);
                Assert.Equal(users[1].Created, u.Created);
                Assert.Equal(users[1].Updated, u.Updated);
            });
    }
}