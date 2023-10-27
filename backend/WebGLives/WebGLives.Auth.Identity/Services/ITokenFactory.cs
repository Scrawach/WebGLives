using CSharpFunctionalExtensions;
using WebGLives.Core.Errors;

namespace WebGLives.Auth.Identity.Services;

public interface ITokenFactory
{
    Task<Result<Tokens, Error>> Create(string login, string password);
    Task<Result<Tokens, Error>> Refresh(int userId, string refreshToken);
}