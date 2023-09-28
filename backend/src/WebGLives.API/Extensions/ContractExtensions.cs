using WebGLives.API.Contracts;
using WebGLives.BusinessLogic;
using WebGLives.Core;

namespace WebGLives.API.Extensions;

public static class ContractExtensions
{
    public static UpdateGameData ToData(this UpdateGameRequest request) =>
        new()
        {
            Title = request.Title,
            Description = request.Description,
            Game = request.Game?.ToData(),
            Poster = request.Poster?.ToData()
        };

    public static FileData ToData(this IFormFile file) =>
        new(
            file.FileName,
            file.OpenReadStream()
        );
}