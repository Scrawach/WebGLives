namespace WebGLives.API.Contracts;

public class AuthenticatedResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}