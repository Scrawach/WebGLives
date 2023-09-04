using Microsoft.EntityFrameworkCore;
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
    public static IServiceCollection AddWebGLives(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<GamesDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
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