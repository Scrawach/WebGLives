using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebGLives.Auth.Identity.Errors;
using WebGLives.Auth.Identity.Extensions;
using WebGLives.Auth.Identity.Options;
using WebGLives.Core;
using WebGLives.Core.Errors;
using WebGLives.Core.Repositories;

namespace WebGLives.Auth.Identity.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UserManager<User> _userManager;
    private readonly IOptions<UsersRepositoryOptions> _options;

    public UsersRepository(UserManager<User> userManager, IOptions<UsersRepositoryOptions> options)
    {
        _userManager = userManager;
        _options = options;
    }

    public async Task<Result<IUser, Error>> CreateAsync(string username, string password) =>
        await _userManager.CreateWithResultAsync(new User(username), password);

    public async Task<Result<IUser, Error>> FindByNameAsync(string username) =>
        await _userManager.FindByNameAsync(username)
            .ToResultAsync<User, Error>(new UserNotFound(username))
            .Map<User, IUser, Error>(user => user);

    public async Task<Result<IUser, Error>> FindByIdAsync(int userId) =>
        await _userManager.FindByIdAsync(userId.ToString())
            .ToResultAsync<User, Error>(new UserNotFound(userId))
            .Map<User, IUser, Error>(user => user);

    public async Task<UnitResult<Error>> CheckPasswordAsync(IUser user, string password) =>
        await Find(user).Ensure(IsCorrectPassword(password), new InvalidPasswordError());

    public async Task<Result<string, Error>> GetAuthenticationTokenAsync(IUser user) =>
        await Find(user).Bind(AuthenticationTokenAsync);

    public async Task<UnitResult<Error>> RemoveAuthenticationTokenAsync(IUser user) =>
        await Find(user).Ensure(RemoveAuthenticationTokenAsync, new RemoveTokenError(user.UserName));

    public async Task<UnitResult<Error>> SetAuthenticationTokenAsync(IUser user, string token) =>
        await Find(user).Ensure(SetAuthenticationTokenAsync(token), new SetTokenError(user.UserName));
    
    private Task<Result<User, Error>> Find(IUser user) =>
        _userManager
            .FindByNameAsync(user.UserName)
            .ToResultAsync<User, Error>(new UserNotFound(user.UserName));

    private Func<User, Task<bool>> IsCorrectPassword(string password) =>
        async entity => await _userManager.CheckPasswordAsync(entity, password);

    private async Task<Result<string, Error>> AuthenticationTokenAsync(User entity) =>
        await _userManager
            .GetAuthenticationTokenAsync(entity, _options.Value.LocalProvider, _options.Value.RefreshTokenTable)
            .ToResultAsync<string, Error>(new GetTokenError(entity.UserName!));

    private async Task<bool> RemoveAuthenticationTokenAsync(User entity)
    {
        var result = await _userManager.RemoveAuthenticationTokenAsync(entity, _options.Value.LocalProvider, _options.Value.RefreshTokenTable);
        return result.Succeeded;
    }
    
    private Func<User, Task<bool>> SetAuthenticationTokenAsync(string token) =>
        async entity =>
        {
            var result = await _userManager.SetAuthenticationTokenAsync(entity, _options.Value.LocalProvider, _options.Value.RefreshTokenTable, token);
            return result.Succeeded;
        };
}