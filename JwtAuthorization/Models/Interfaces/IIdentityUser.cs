namespace JwtAuthorization.Models.Interfaces;

public interface IIdentityUser
{
    Guid Id { get; set; }
    string? UserName { get; set; }
    ICollection<IRole>? Roles { get; set; }
}