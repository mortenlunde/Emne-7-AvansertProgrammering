namespace JwtAuthorization.Models.Interfaces;

public interface IRole
{
    Guid? Id { get; set; }
    string? Name { get; set; }
}