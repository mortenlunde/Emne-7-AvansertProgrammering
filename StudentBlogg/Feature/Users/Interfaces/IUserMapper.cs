namespace StudentBlogg.Feature.Users.Interfaces;

public interface IUserMapper
{
    UserDto MaapToDto(User model);
    User MapToModel(UserDto dto);
}