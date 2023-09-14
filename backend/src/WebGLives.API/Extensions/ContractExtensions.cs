using WebGLives.API.Requests;
using WebGLives.BusinessLogic.Services.Abstract;

namespace WebGLives.API.Extensions;

public static class ContractExtensions
{
    public static UpdateGameData ToData(this UpdateGameRequest request) =>
        new()
        {
            Title = request.Title,
            Description = request.Description,
            Game = request.Game?.OpenReadStream(),
            Poster = request.Poster?.OpenReadStream()
        };
}