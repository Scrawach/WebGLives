using CSharpFunctionalExtensions;
using WebGLives.API.Contracts;
using WebGLives.Core.Errors;

namespace WebGLives.API.Services;

public interface ITokenFactory
{
    Task<Result<AuthenticatedResponse, Error>> Create(LoginRequest credentials);
    Task<UnitResult<Error>> Decode(string token);
    Task<Result<AuthenticatedResponse, Error>> Refresh(TokenRefreshRequest credentials);
}