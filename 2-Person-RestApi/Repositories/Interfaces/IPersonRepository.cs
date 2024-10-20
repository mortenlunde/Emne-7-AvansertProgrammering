using Person_RestApi.Models;
namespace Person_RestApi.Repositories.Interfaces;

public interface IPersonRepository
{
    Task<Person?> AddPersonAsync(Person person);
    Task<ICollection<Person>> GetAllAsync();
    Task<Person?> DeleteByIdAsync(long id);
    Task<Person?> UpdateByIdAsync(long id, Person person);
    Task<Person?> GetByIdAsync(long id);
}