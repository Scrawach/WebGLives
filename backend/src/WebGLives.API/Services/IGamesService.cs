using CSharpFunctionalExtensions;
using WebGLives.Core;

namespace WebGLives.API.Services;

public interface IGamesService
{
    Task<Result<IEnumerable<Game>>> All();
    Task<Result> Create(Game newGame);
    Task<Result<Game>> Get(int id);
    Task<Result> Delete(int id);
}