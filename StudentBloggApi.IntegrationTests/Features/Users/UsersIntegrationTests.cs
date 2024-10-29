using System.Net;
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
        
        // Assign
        var response = await _client.GetAsync("/api/v1/Users");
        
        // Assert
        var data = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(await response.Content.ReadAsStringAsync());

        Assert.NotNull(data);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}