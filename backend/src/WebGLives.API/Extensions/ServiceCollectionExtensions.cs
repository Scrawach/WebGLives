using JWT.Algorithms;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebGLives.API.Services;
using WebGLives.Auth.Identity.Services;
using WebGLives.BusinessLogic.Services;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core.Repositories;
using WebGLives.Core.Users;
using WebGLives.DataAccess;
using WebGLives.DataAccess.Repositories;

namespace WebGLives.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebGLives(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.Seq(builder.Configuration.GetConnectionString(nameof(Serilog))!)
            .CreateLogger();
        
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(logger, dispose: true);
        });
        
        services.AddDbContext<GamesDbContext>(options =>
        {
            options.UseNpgsql
            (
                builder.Configuration.GetConnectionString(nameof(GamesDbContext)),
                npgsqlOptions => npgsqlOptions.MigrationsAssembly("WebGLives.Migrations")
            );
        });
        
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<DataAccessMappingProfile>();
        });

        services.AddSingleton<IZipService, ZipService>();
        services.AddSingleton<IFilesService, FilesService>();
        services.AddSingleton<IJwtTokenService>(new JwtTokenService(builder.Configuration.GetAuthenticationSecretKey(), new HMACSHA256Algorithm()));
        services.AddSingleton<IAuthorizedGameService, AuthorizedGameService>();
        services.AddScoped<ITokenFactory, TokenFactory>();
        services.AddScoped<IGamesService, GamesService>();
        services.AddScoped<IGamesRepository, GamesRepository>();
        services.AddScoped<IUsersService, UsersService>();

        return services;
    }
}