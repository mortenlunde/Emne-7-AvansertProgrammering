using JwtAuthorization.Models.Interfaces;

namespace JwtAuthorization.Services.Interfaces;

public interface ITokenService
{
    Task<IIdentityUser> LoginAsync(string username, string password);
    Task<string?> GenerateJwtTokenAsync(IIdentityUser user);
}