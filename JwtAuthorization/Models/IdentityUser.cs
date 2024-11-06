using JwtAuthorization.Models.Interfaces;

namespace JwtAuthorization.Models;

public class IdentityUser : IIdentityUser
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public ICollection<IRole> Roles { get; set; }
}