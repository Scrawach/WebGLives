using CSharpFunctionalExtensions;
using WebGLives.Core.Errors;

namespace WebGLives.Core.Repositories;

public interface IRefreshTokensRepository
{
    Task<UnitResult<Error>> CreateToken(IUser owner, string token);
    Task<Result<string, Error>> GetToken(IUser owner);
    Task<UnitResult<Error>> RemoveToken(IUser owner);
}