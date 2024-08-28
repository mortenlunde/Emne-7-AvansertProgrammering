using Person_RestApi.Endpoints;
using Person_RestApi.Models;
using Person_RestApi.Repositories;
using Person_RestApi.Repositories.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Vi legger til vår service;
builder.Services.AddSingleton<IPersonRepository, PersonInMemoryDataStorage>();
builder.Services.AddSingleton<IRepository<Person>, PersonGenericInMemDb>();
// builder.Services.AddScoped<>();
// builder.Services.AddTransient<>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Lager vårt første endepunkt. Metode: GET,  https://localhost:7234/persons/
app.MapPersonEndpoints();

app.Run();