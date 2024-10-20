using StudentBlogg.Feature.Users.Interfaces;

namespace StudentBlogg.Feature.Users;

public class UserMapper : IUserMapper
{
    public UserDto MaapToDto(User model)
    {
        return new UserDto()
        {
            Id = model.Id,
            Username = model.Username,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Created = model.Created,
            Updated = model.Updated,
        };
    }

    public User MapToModel(UserDto dto)
    {
        return new User
        {
            Id = dto.Id,
            Username = dto.Username,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Created = dto.Created,
            Updated = dto.Updated,
        };
    }
}