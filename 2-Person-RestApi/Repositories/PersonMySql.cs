using System.Data.Common;
using MySql.Data.MySqlClient;
using Person_RestApi.Models;
using Person_RestApi.Repositories.Interfaces;

namespace Person_RestApi.Repositories;

public class PersonMySql(IConfiguration configuration) : IPersonRepository
{
    private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");

    public async Task<Person?> AddPersonAsync(Person person)
    {
        await using MySqlConnection conn = new(_connectionString);
        conn.Open();
        
        string query = "INSERT INTO Person (firstName, lastName, Age) VALUES (@firstName, @lastName, @age)";
        MySqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@firstName", person.FirstName);
        cmd.Parameters.AddWithValue("@lastName", person.LastName);
        cmd.Parameters.AddWithValue("@age", person.Age);
        
        
        await cmd.ExecuteNonQueryAsync();
        cmd.CommandText = "SELECT LAST_INSERT_ID()";
        person.Id = Convert.ToInt64(cmd.ExecuteScalar());
        return person;
    }

    public async Task<ICollection<Person>> GetAllAsync()
    {
        List<Person> people = [];
        await using MySqlConnection conn = new(_connectionString);
        conn.Open();
        const string query = "SELECT id, firstname as Fornavn, lastname AS Etternavn, age as Alder FROM Person";
        MySqlCommand cmd = new(query, conn);
        await using DbDataReader reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Person person = new()
            {
                Id = reader.GetInt64(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Age = reader.GetInt32(3),
            };
            people.Add(person);
        }
        return people;
    }

    public async Task<Person?> DeleteByIdAsync(long id)
    {
        Person personToDelete = (await GetByIdAsync(id))!;
        await using MySqlConnection conn = new(_connectionString);
        conn.Open();
        string query = "DELETE FROM Person WHERE id = @id";
        MySqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@id", id);
        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        if (rowsAffected > 0)
            return personToDelete;

        return null;
    }

    public async Task<Person?> UpdateByIdAsync(long id, Person person)
    {
        await using MySqlConnection conn = new(_connectionString);
        conn.Open();
        
        string query = "UPDATE Person SET firstName = @firstName, lastName = @lastName, age = @age WHERE Id = @Id";
        MySqlCommand cmd = new(query, conn);
        
        cmd.Parameters.AddWithValue("@firstName", person.FirstName);
        cmd.Parameters.AddWithValue("@lastName", person.LastName);
        cmd.Parameters.AddWithValue("@age", person.Age);
        cmd.Parameters.AddWithValue("@Id", person.Id);

        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        if (rowsAffected > 0)
            return null;
        return await GetByIdAsync(id);
    }

    public async Task<Person?> GetByIdAsync(long id)
    {
        await using MySqlConnection conn = new(_connectionString);
        conn.Open();
        string query = "SELECT id, firstName AS Fornavn, lastName AS Etternavn, age AS Alder FROM Person WHERE id = @id";
        MySqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@id", id);
        await using DbDataReader reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Person
            {
                Id = reader.GetInt64(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Age = reader.GetInt32(3),
            };
        }
        return null;
    }
}