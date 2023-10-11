using CSharpFunctionalExtensions;
using WebGLives.Core.Errors;

namespace WebGLives.Core.Users;

public interface IUserService
{
    Task<UnitResult<Error>> CreateAsync(string username, string password);
    Task<Result<IUser, Error>> FindByNameAsync(string username);
    Task<UnitResult<Error>> CheckPasswordAsync(IUser user, string password);
    Task<Result<string, Error>> GetAuthenticationTokenAsync(IUser user);
    Task<UnitResult<Error>> RemoveAuthenticationTokenAsync(IUser user);
    Task<UnitResult<Error>> SetAuthenticationTokenAsync(IUser user, string token);
}