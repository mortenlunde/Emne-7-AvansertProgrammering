using Person_RestApi.Models;
using Person_RestApi.Repositories.Interfaces;
namespace Person_RestApi.Endpoints;

public static class EndpointExtensions
{
    public static void MapGenericEndpoints(this WebApplication app)
    {
        RouteGroupBuilder personGroup = app.MapGroup("/persons");
        personGroup.MapGet("", GetPersonsAsync).WithName("GetPersonsAsync").WithOpenApi();
        personGroup.MapPost("", PostPersonsAsync).WithName("PostPersonsAsync").WithOpenApi();
        personGroup.MapDelete("/{id:long}", DeleteAsync).WithName("DeleteAsync").WithOpenApi();
        personGroup.MapPatch("/{id:long}", UpdatePersonsAsync).WithName("UpdatePersonsAsync").WithOpenApi();
    }

    private static async Task<IResult> GetPersonsAsync(IRepository<Person> repo)
    {
        ICollection<Person> persons = await repo.GetAllAsync();
        return Results.Ok(persons);
    }

    private static async Task<IResult> PostPersonsAsync(IRepository<Person> repo, Person person)
    {
        Person? newPerson = await repo.AddAsync(person);
        return newPerson is null
            ? Results.BadRequest("Failed to add person to database.")
            : Results.Ok(newPerson);
    }

    private static async Task<IResult> DeleteAsync(IRepository<Person> repo, long id)
    {
        Person? personToDelete = await repo.DeleteByIdAsync(id);
        if (personToDelete is null)
        {
            return Results.NotFound($"Person with ID: {id} not found.");
        }

        Person? deletedPerson = await repo.DeleteAsync(personToDelete);
        return deletedPerson is null
            ? Results.BadRequest($"Failed to delete person with ID: {id} from database.")
            : Results.Ok(deletedPerson);
    }


    private static async Task<IResult> UpdatePersonsAsync(IRepository<Person> repo, long id, Person person)
    {
        Person? existingPerson = await repo.GetByIdAsync(id);
        if (existingPerson is null)
            return Results.BadRequest($"Person with ID: {id} not found.");

        person.Id = id;  // Ensure the ID remains the same during the update
        Person? updatedPerson = await repo.UpdateAsync(person);
            
        return updatedPerson is null
            ? Results.BadRequest($"Failed to update person with ID: {id} in the database.")
            : Results.Ok(updatedPerson);
    }
}