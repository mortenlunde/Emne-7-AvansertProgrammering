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
        personGroup.MapDelete("/{id:int}", DeletePersonsAsync).WithName("DeletePersonsAsync").WithOpenApi();
        personGroup.MapPatch("/{id:int}", UpdatePersonsAsync).WithName("UpdatePersonsAsync").WithOpenApi();
    }

    private static async Task<IResult> GetPersonsAsync(IPersonRepository repo)
    {
        // Hente fra databasen
        return Results.Ok(await repo.GetAllAsync());
    }

    private static async Task<IResult> PostPersonsAsync(IPersonRepository repo, Person person)
    {
        Person? p = await repo.AddPersonAsync(person);
        
        return p is null
            ? Results.BadRequest("Failed to add person to database.")
            : Results.Ok(p);
    }

    private static async Task<IResult> DeletePersonsAsync(IPersonRepository repo, int id)
    {
        Person? p = await repo.DeleteByIdAsync(id);
        
        return p is null
            ? Results.BadRequest($"Failed to delete person with ID: {id} from database.")
            : Results.Ok(p);
    }

    private static async Task<IResult> UpdatePersonsAsync(IPersonRepository repo, int id, Person person)
    {
        Person? p = await repo.UpdateByIdAsync(id, person);
        
        return p is null
            ? Results.BadRequest($"Failed to update person with ID: {id} from database.")
            : Results.Ok(p);
    }
}