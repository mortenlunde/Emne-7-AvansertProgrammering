using Person_RestApi.Models;
namespace Person_RestApi.Repositories.Interfaces;

public interface IPersonRepository
{
    Task<Person?> AddPersonAsync(Person person);
    Task<ICollection<Person>> GetAllAsync();
    Task<Person?> DeleteByIdAsync(int id);
    Task<Person?> UpdateByIdAsync(int id, Person person);
}