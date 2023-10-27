using CSharpFunctionalExtensions;
using WebGLives.Core.Errors;

namespace WebGLives.Core.Repositories;

public interface IUsersRepository
{
    Task<Result<IUser, Error>> CreateAsync(string username, string password);
    Task<Result<IUser, Error>> FindByNameAsync(string username);
    Task<Result<IUser, Error>> FindByIdAsync(int userId);
    Task<UnitResult<Error>> CheckPasswordAsync(IUser user, string password);
}