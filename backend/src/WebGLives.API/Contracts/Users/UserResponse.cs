using CSharpFunctionalExtensions;
using WebGLives.Core;
using WebGLives.Core.Errors;

namespace WebGLives.API.Contracts.Users;

public class UserResponse
{
    public int Id { get; set; }
    public string Login { get; set; }
    
    public static UserResponse From(IUser user) =>
        new()
        {
            Id = user.Id,
            Login = user.UserName
        };
    
    public static Task<Result<UserResponse, Error>> From(Task<Result<IUser, Error>> tokens) =>
        tokens.Map(From);
}