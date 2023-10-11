namespace WebGLives.API.Contracts.Auth;

public class TokenRefreshRequest
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}