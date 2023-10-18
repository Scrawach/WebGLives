using System.Text;
using Microsoft.AspNetCore.Identity;
using WebGLives.Core.Errors;

namespace WebGLives.Auth.Identity.Errors;

public class CreateUserError : Error
{
    public CreateUserError(IdentityResult result) : base(ConvertToString(result)) { }

    private static string ConvertToString(IdentityResult result)
    {
        var builder = new StringBuilder();
        
        foreach (var error in result.Errors) 
            builder.Append($"[{error.Code}] {error.Description}\n");

        return builder.ToString();
    }
}