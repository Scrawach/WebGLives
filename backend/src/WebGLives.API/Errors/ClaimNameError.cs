using WebGLives.Core.Errors;

namespace WebGLives.API.Errors;

public class ClaimNameError : Error
{
    public ClaimNameError() : base("Authorized User has not Name Identifier!") { }
}