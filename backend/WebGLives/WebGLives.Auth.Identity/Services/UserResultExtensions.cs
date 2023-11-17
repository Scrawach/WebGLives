using CSharpFunctionalExtensions;
using WebGLives.Core;
using WebGLives.Core.Errors;
using WebGLives.Core.Repositories;

namespace WebGLives.Auth.Identity.Services;

public static class UserResultExtensions
{
    public static Task<Result<IUser, Error>> CheckPassword(this Task<Result<IUser, Error>> self, IUsersRepository repository, string password) =>
        self.Check(async entity => await repository.CheckPasswordAsync(entity, password));

    public static Task<Result<Tokens, Error>> Authenticate(this Task<Result<IUser, Error>> self, Func<IUser, Task<Tokens>> tokens) =>
        self.Map(async user => await tokens(user));
}