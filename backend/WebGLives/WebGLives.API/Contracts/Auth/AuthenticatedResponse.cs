using CSharpFunctionalExtensions;
using WebGLives.Auth.Identity;
using WebGLives.Core.Errors;

namespace WebGLives.API.Contracts.Auth;

public class AuthenticatedResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpireAt { get; set; }
    
    public static AuthenticatedResponse From(Tokens tokens) =>
        new()
        {
           AccessToken = tokens.Access,
           RefreshToken = tokens.Refresh,
           ExpireAt = tokens.ExpireAt
        };
    
    public static Task<Result<AuthenticatedResponse, Error>> From(Task<Result<Tokens, Error>> tokens) =>
        tokens.Map(From);
}