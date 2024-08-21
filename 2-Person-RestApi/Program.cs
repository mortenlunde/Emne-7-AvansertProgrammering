using Person_RestApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Lager vårt første endepunkt. Metode: GET,  https://localhost:7234/persons/
app.MapGet("/persons", () =>
    {
        Person person = new Person{age = 33, FirstName = "John", LastName = "Doe", Id = 1};
        
        return Results.Ok(person);
    }).WithName("Persons")
    .WithOpenApi();

app.Run();