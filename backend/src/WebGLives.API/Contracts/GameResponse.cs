using CSharpFunctionalExtensions;
using WebGLives.Core;
using WebGLives.Core.Errors;

namespace WebGLives.API.Contracts;

public class GameResponse
{
    public int Id { get; set; }
    
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? PosterUrl { get; set; }

    public string? GameUrl { get; set; }

    public static GameResponse From(Game game) =>
        new()
        {
            Id = game.Id,
            Title = game.Title,
            Description = game.Description,
            PosterUrl = game.PosterUrl,
            GameUrl = game.GameUrl
        };
    
    public static Task<Result<GameResponse, Error>> From(Task<Result<Game, Error>> game) =>
        game.Map(GameResponse.From);

    public static Task<Result<IEnumerable<GameResponse>, Error>> From(Task<Result<IEnumerable<Game>, Error>> games) =>
        games.Map(datas => datas.Select(GameResponse.From));
}