using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using WebGLives.Core.Errors;
using WebGLives.Core.Repositories;
using WebGLives.Core.Users;

namespace WebGLives.Auth.Identity.Repositories;

public class UsersRepository : IUsersRepository
{
    private const string LocalProvider = "LocalLogin";
    private const string RefreshTokenName = "RefreshToken";

    private readonly UserManager<User> _userManager;

    public UsersRepository(UserManager<User> userManager) =>
        _userManager = userManager;

    public async Task<UnitResult<Error>> CreateAsync(string username, string password)
    {
        var result = await _userManager.CreateAsync(new User(username), password);
        return result.Succeeded
            ? UnitResult.Success<Error>()
            : UnitResult.Failure(new Error("Create user error"));
    }

    public async Task<Result<IUser, Error>> FindByNameAsync(string username)
    { 
        var user = await _userManager.FindByNameAsync(username);
        return user != null 
            ? Result.Success<IUser, Error>(user) 
            : Result.Failure<IUser, Error>(new UserNotFound(username));
    }
    
    public async Task<UnitResult<Error>> CheckPasswordAsync(IUser user, string password) =>
        await Find(user).Ensure(IsCorrectPassword(password), new Error("Invalid password!"));

    public async Task<Result<string, Error>> GetAuthenticationTokenAsync(IUser user) =>
        await Find(user).Bind(AuthenticationTokenAsync);

    public async Task<UnitResult<Error>> RemoveAuthenticationTokenAsync(IUser user) =>
        await Find(user).Ensure(RemoveAuthenticationTokenAsync, new Error($"Can't remove token for {user.UserName}!"));

    public async Task<UnitResult<Error>> SetAuthenticationTokenAsync(IUser user, string token) =>
        await Find(user).Ensure(SetAuthenticationTokenAsync(token), new Error($"Can't set token for {user.UserName}!"));
    
    private Task<Result<User, Error>> Find(IUser user) =>
        _userManager
            .FindByNameAsync(user.UserName)
            .ToResultAsync<User, Error>(new UserNotFound(user.UserName));

    private Func<User, Task<bool>> IsCorrectPassword(string password) =>
        async entity => await _userManager.CheckPasswordAsync(entity, password);

    private async Task<Result<string, Error>> AuthenticationTokenAsync(User entity) =>
        await _userManager
            .GetAuthenticationTokenAsync(entity, LocalProvider, RefreshTokenName)
            .ToResultAsync(new Error($"Can't get token for {entity.UserName}"));

    private async Task<bool> RemoveAuthenticationTokenAsync(User entity)
    {
        var result = await _userManager.RemoveAuthenticationTokenAsync(entity, LocalProvider, RefreshTokenName);
        return result.Succeeded;
    }
    
    private Func<User, Task<bool>> SetAuthenticationTokenAsync(string token) =>
        async entity =>
        {
            var result = await _userManager.SetAuthenticationTokenAsync(entity, LocalProvider, RefreshTokenName, token);
            return result.Succeeded;
        };
}