using Microsoft.EntityFrameworkCore;
using Serilog;
using StudentBlogg.Common.Interfaces;
using StudentBlogg.Configurations;
using StudentBlogg.Data;
using StudentBlogg.Extensions;
using StudentBlogg.Feature.Users;
using StudentBlogg.Feature.Users.Interfaces;
using StudentBlogg.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen();
    builder.Services.AddSwaggerBasicAuthentication();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IMapper<User, UserDto>, UserMapperGeneric>();
    builder.Services.AddScoped<IMapper<User, UserRegistrationDto>, UserRegMapper>();
    builder.Services.AddDbContext<StudentBloggDbContext>(options => 
        options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion(new Version(8, 0, 33))));
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<StudentBlogBasicAuthentication>();
    builder.Services.Configure<BasicAuthenticationOptions>(builder.Configuration.GetSection("BasicAuthenticationOptions"));
    builder.Services.AddHttpContextAccessor();

    builder.Host.UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    });
}
WebApplication app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app
            
        .UseMiddleware<StudentBlogBasicAuthentication>()
        .UseHttpsRedirection()
        .UseAuthorization();
    
    app.MapControllers();

    app.Run();
}