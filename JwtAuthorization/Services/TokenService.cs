using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuthorization.Configuration;
using JwtAuthorization.Models;
using JwtAuthorization.Models.Interfaces;
using JwtAuthorization.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthorization.Services;

public class TokenService : ITokenService
{
    private readonly IOptions<JwtOptions> _options;

    public TokenService(IOptions<JwtOptions> options)
    {
        _options = options;
    }
    public async Task<IIdentityUser> LoginAsync(string username, string password)
    {
        await Task.Delay(10);
        return new IdentityUser()
        {
            Id = Guid.NewGuid(),
            UserName = username,
            Roles =
            [
                new Role() { Id = Guid.NewGuid(), Name = "Admin" },
                new Role() { Id = Guid.NewGuid(), Name = "Developer" }
            ]
        };
    }

    public async Task<string?> GenerateJwtTokenAsync(IIdentityUser user)
    {
        await Task.Delay(10);
        List<Claim> claims = 
        [
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.NameIdentifier, user.Id.ToString()!),
            new (ClaimTypes.Name, user.UserName!)
        ];

        foreach (IRole role in user.Roles!)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name!));
        }
        
        byte[] secrets = Encoding.UTF8.GetBytes(_options.Value.Key!);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _options.Value.Issuer,
            Audience = _options.Value.Audience,
            SigningCredentials = 
                new SigningCredentials(new SymmetricSecurityKey(secrets), SecurityAlgorithms.HmacSha256Signature)
        };
        
        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}