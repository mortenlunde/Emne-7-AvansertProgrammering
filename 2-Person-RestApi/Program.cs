using Person_RestApi.Endpoints;
using Person_RestApi.Middleware;
using Person_RestApi.Repositories;
using Person_RestApi.Repositories.Interfaces;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen()
        .AddExceptionHandler<GlobalExceptionHandling>()
        // .AddSingleton<IRepository<Person>, PersonGenericInMemDb>()
        // .AddSingleton<IPersonRepository, PersonInMemoryDataStorage>();
        .AddSingleton<IPersonRepository, PersonMySql>();
    // builder.Services.AddScoped<>();
    // builder.Services.AddTransient<>();

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("Logs/log-.txt", rollingInterval:RollingInterval.Day)
        .CreateLogger();
    
    builder.Host.UseSerilog();
}

WebApplication app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Registrering av Middleware
    app
        .UseHttpsRedirection()
        .UseExceptionHandler(_ => { });

    // Lager vårt første endepunkt. Metode: GET,  https://localhost:7234/persons/
    app.MapPersonEndpoints();
    // app.MapGenericEndpoints();
}

app.Run();