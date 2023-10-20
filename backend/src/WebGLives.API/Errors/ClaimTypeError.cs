using WebGLives.Core.Errors;

namespace WebGLives.API.Errors;

public class ClaimTypeError : Error
{
    public ClaimTypeError() : base("Invalid Name Identifier claim format!") { }
}