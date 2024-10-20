using Person_RestApi.Models;
using Person_RestApi.Repositories.Interfaces;

namespace Person_RestApi.Repositories;

public class PersonInMemoryDataStorage : IPersonRepository
{
    private static readonly List<Person> DbInMemStorage = [];
    public async Task<Person?> AddPersonAsync(Person person)
    {
        // Legge til en liste
        await Task.Delay(10);
        person.Id = DbInMemStorage.Count + 1;
        DbInMemStorage.Add(person);
        return person;
    }
    

    public async Task<ICollection<Person>> GetAllAsync()
    {
        // Hente alt fra listen
        await Task.Delay(10);
        return DbInMemStorage;
    }

    public async Task<Person?> DeleteByIdAsync(long id)
    {
        await Task.Delay(10);
        Person? person = DbInMemStorage.FirstOrDefault(p => p.Id == id);
        if (person != null)
            DbInMemStorage.Remove(person);
        return person;
    }

    public Task<Person?> UpdateByIdAsync(long id, Person person)
    {
        throw new NotImplementedException();
    }

    public Task<Person?> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<Person?> UpdateByIdAsync(int id, Person updatedPerson)
    {
        await Task.Delay(10);
        Person? existingPerson = DbInMemStorage.FirstOrDefault(p => p.Id == id);
        if (existingPerson == null) return null;
        existingPerson.FirstName = updatedPerson.FirstName;
        existingPerson.LastName = updatedPerson.LastName;
        existingPerson.Age = updatedPerson.Age;

        return existingPerson;
    }
}