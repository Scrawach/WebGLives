using CSharpFunctionalExtensions;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core;
using WebGLives.Core.Errors;

namespace WebGLives.BusinessLogic.Extensions;

public static class GameResultExtensions
{
    public static Task<Result<Game, Error>> CheckAndSaveGameFile(this Task<Result<Game, Error>> self, FileData? data, 
        IFilesService files, CancellationToken token = default) =>
        self.CheckIf(data is not null, async game => await files
            .SaveZip(game.Id.ToString(), data!, token)
            .Tap(path => game.GameUrl = path));

    public static Task<Result<Game, Error>> CheckAndSavePosterFile(this Task<Result<Game, Error>> self, FileData? poster,
        IFilesService files, CancellationToken token = default) =>
        self.CheckIf(poster is not null, async game => await files
            .Save(game.Id.ToString(), poster!, token)
            .Tap(path => game.PosterUrl = path));
}