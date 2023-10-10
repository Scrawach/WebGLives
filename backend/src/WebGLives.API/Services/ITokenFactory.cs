using CSharpFunctionalExtensions;
using WebGLives.API.Contracts;
using WebGLives.Core.Errors;

namespace WebGLives.API.Services;

public interface ITokenFactory
{
    Task<Result<AuthenticatedResponse, Error>> Create(string login, string password);
    Task<Result<AuthenticatedResponse, Error>> Refresh(string accessToken, string refreshToken);
}