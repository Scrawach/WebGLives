using WebGLives.API.Services;
using WebGLives.API.Services.Abstract;
using WebGLives.BusinessLogic.Services;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.DataAccess;
using WebGLives.DataAccess.Repositories;

namespace WebGLives.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebGLives(this IServiceCollection collection)
    {
        collection.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<DataAccessMappingProfile>();
        });

        collection.AddSingleton<IZipService, ZipService>();
        collection.AddSingleton<IFilesService, FilesService>();
        collection.AddSingleton<IGamesService, GamesService>();
        collection.AddScoped<IGamesRepository, GamesRepository>();

        return collection;
    }
}