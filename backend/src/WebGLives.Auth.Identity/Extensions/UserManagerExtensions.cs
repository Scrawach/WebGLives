using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using WebGLives.Auth.Identity.Errors;
using WebGLives.Core;
using WebGLives.Core.Errors;

namespace WebGLives.Auth.Identity.Extensions;

public static class UserManagerExtensions
{
    public static async Task<Result<IUser, Error>> CreateAsyncWithResult(this UserManager<User> manager, User user, string password)
    {
        var result = await manager.CreateAsync(user, password);
        return result.Succeeded
            ? Result.Success<IUser, Error>(user)
            : Result.Failure<IUser, Error>(new CreateUserError(result));
    }
}