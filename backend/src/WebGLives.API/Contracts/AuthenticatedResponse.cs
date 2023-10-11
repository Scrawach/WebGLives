using CSharpFunctionalExtensions;
using WebGLives.Auth.Identity;
using WebGLives.Core.Errors;

namespace WebGLives.API.Contracts;

public class AuthenticatedResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    
    public static AuthenticatedResponse From(Tokens tokens) =>
        new()
        {
           AccessToken = tokens.Access,
           RefreshToken = tokens.Refresh
        };
    
    public static Task<Result<AuthenticatedResponse, Error>> From(Task<Result<Tokens, Error>> tokens) =>
        tokens.Map(From);
}