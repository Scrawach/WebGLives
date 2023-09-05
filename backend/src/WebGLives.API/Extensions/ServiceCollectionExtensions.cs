using Microsoft.EntityFrameworkCore;
using Serilog;
using WebGLives.API.Services;
using WebGLives.API.Services.Abstract;
using WebGLives.BusinessLogic.Services;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core.Repositories;
using WebGLives.DataAccess;
using WebGLives.DataAccess.Repositories;

namespace WebGLives.API.Extensions;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebGLives(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341")
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

            options.LogTo(logger.Information);
        });
        
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<DataAccessMappingProfile>();
        });

        services.AddSingleton<IZipService, ZipService>();
        services.AddSingleton<IFilesService, FilesService>();
        services.AddScoped<IGamesService, GamesService>();
        services.AddScoped<IGamesRepository, GamesRepository>();

        return services;
    }
}