using JwtAuthorization.Models.Interfaces;

namespace JwtAuthorization.Models;

public class Role : IRole
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
}