using Person_RestApi.Models;
using Person_RestApi.Repositories.Interfaces;

namespace Person_RestApi.Repositories;

public class PersonGenericInMemDb : IRepository<Person>
{
    private static readonly List<Person> PersonsList = [];
    public async Task<ICollection<Person>> GetAllAsync()
    {
        await Task.Delay(10);
        return PersonsList;
    }

    public async Task<Person?> AddAsync(Person item)
    {
        await Task.Delay(10);
        item.Id = PersonsList.Count + 1;
        PersonsList.Add(item);
        return item;
    }
    
    public Task<Person?> GetByIdAsync(long id)
    {
        Person? existingPerson = PersonsList.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(existingPerson);
    }

    public async Task<Person?> UpdateAsync(Person item)
    {
        await Task.Delay(10);
        Person? existingPerson = await GetByIdAsync(item.Id);
        if (existingPerson == null) return null;
        existingPerson.FirstName = item.FirstName;
        existingPerson.LastName = item.LastName;
        existingPerson.Age = item.Age;

        return existingPerson;
    }

    public async Task<Person?> DeleteAsync(Person item)
    {
        await Task.Delay(10);
        Person? existingPerson = await DeleteByIdAsync(item.Id);
        if (existingPerson != null)
            PersonsList.Remove(existingPerson);
        return existingPerson;
    }

    public Task<Person?> DeleteByIdAsync(long id)
    {
        Person? existingPerson = PersonsList.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(existingPerson);
    }
}