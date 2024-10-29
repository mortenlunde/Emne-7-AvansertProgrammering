using Microsoft.EntityFrameworkCore;
using Serilog;
using StudentBlogg.Common.Interfaces;
using StudentBlogg.Configurations;
using StudentBlogg.Data;
using StudentBlogg.Extensions;
using StudentBlogg.Feature.Posts;
using StudentBlogg.Feature.Posts.Interfaces;
using StudentBlogg.Feature.Users;
using StudentBlogg.Feature.Users.Interfaces;
using StudentBlogg.Middleware;

namespace StudentBlogg;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerBasicAuthentication();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IMapper<User, UserDto>, UserMapperGeneric>();
        builder.Services.AddScoped<IMapper<User, UserRegistrationDto>, UserRegMapper>();
        builder.Services.AddDbContext<StudentBloggDbContext>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 33)),
                mySqlOptions => mySqlOptions.EnableRetryOnFailure(2)));
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<BasicAuthentication>();
        builder.Services.Configure<BasicAuthenticationOptions>(builder.Configuration.GetSection("BasicAuthenticationOptions"));
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddExceptionHandler<GlobalExceptionHandling>();
        builder.Services.AddScoped<DatabaseConnectionMiddleware>();

        builder.Services.AddScoped<IPostService, PostService>();
        builder.Services.AddScoped<IMapper<Post, PostDto>, PostMapper>();
        builder.Services.AddScoped<IMapper<Post, PostRegDto>, PostRegMapper>();
        builder.Services.AddScoped<IPostRepository, PostRepository>();

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app
            .UseMiddleware<DatabaseConnectionMiddleware>()
            .UseExceptionHandler(_ => { }) 
            .UseMiddleware<BasicAuthentication>()
            .UseHttpsRedirection()
            .UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}