using Microsoft.AspNetCore.Mvc;
using Person_RestApi.Models;
using Person_RestApi.Repositories.Interfaces;

namespace Person_RestApi.Endpoints;

public static class PersonEndpoints
{
    public static void MapPersonEndpoints(this WebApplication app)
    {
        RouteGroupBuilder personGroup = app.MapGroup("/persons");
        personGroup.MapGet("", GetPersonsAsync).WithName("GetPersonsAsync").WithOpenApi();
        personGroup.MapPost("", PostPersonsAsync).WithName("PostPersonsAsync").WithOpenApi();
        personGroup.MapDelete("/{id:long}", DeletePersonsAsync).WithName("DeletePersonsAsync").WithOpenApi();
        personGroup.MapPatch("/{id:long}", UpdatePersonsAsync).WithName("UpdatePersonsAsync").WithOpenApi();
    }

    private static async Task<IResult> GetPersonsAsync([FromServices]IPersonRepository repo, [FromQuery] long? id)
    {
        ICollection<Person> persons = await repo.GetAllAsync();
        // Hente fra databasen
        return id is null
            ? Results.Ok(persons)
            : Results.Ok(persons.Where(p => p.Id == id));
    }

    private static async Task<IResult> PostPersonsAsync(
        IPersonRepository repo,
        ILogger<Program> logger,
        Person person)
    {
        logger.LogInformation("Person added: {@Person}", person);
        Person? p = await repo.AddPersonAsync(person);
        
        return p is null
            ? Results.BadRequest("Failed to add person to database.")
            : Results.Ok(p);
    }

    private static async Task<IResult> DeletePersonsAsync(IPersonRepository repo, long id)
    {
        Person? p = await repo.DeleteByIdAsync(id);
        
        return p is null
            ? Results.BadRequest($"Failed to delete person with ID: {id} from database.")
            : Results.Ok(p);
    }

    private static async Task<IResult> UpdatePersonsAsync(IPersonRepository repo, long id, Person person)
    {
        Person? p = await repo.UpdateByIdAsync(id, person);
        
        return p is null
            ? Results.BadRequest($"Failed to update person with ID: {id} from database.")
            : Results.Ok(p);
    }
}