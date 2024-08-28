using Person_RestApi.Models;
using Person_RestApi.Repositories.Interfaces;

namespace Person_RestApi.Repositories;

public class PersonGenericInMemDb : IRepository<Person>
{
    public Task<ICollection<Person>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Person?> AddAsync(Person item)
    {
        throw new NotImplementedException();
    }

    public Task<Person?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Person?> UpdateAsync(Person item)
    {
        throw new NotImplementedException();
    }

    public Task<Person?> DeleteAsync(Person item)
    {
        throw new NotImplementedException();
    }

    public Task<Person?> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}