using Person_RestApi.Models;

namespace Person_RestApi.Endpoints;

public static class PersonEndpoints
{
    public static void MapPersonEndpoints(this WebApplication app)
    {
        app.MapGet("/persons", GetPersons)
            .WithName("GetPersons")
            .WithOpenApi();

        app.MapPost("/persons", PostPersons)
            .WithName("PostPersons")
            .WithOpenApi();
    }

    private static IResult GetPersons()
    {
        List<Person> persons =
        [
            new Person { Id = 1, FirstName = "John", LastName = "Doe", Age = 30},
            new Person { Id = 2, FirstName = "Jane", LastName = "Dough", Age = 28},
            new Person { Id = 3, FirstName = "Julie", LastName = "Dooooh", Age = 25},
            new Person { Id = 4, FirstName = "Karen", LastName = "Donut", Age = 22 },
        ];

        return Results.Ok(persons);
    }

    private static IResult PostPersons(Person person)
    {
        return Results.Ok(new Person()
        {
            Age = person.Age + 1,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Id = person.Id
        });
    }
}