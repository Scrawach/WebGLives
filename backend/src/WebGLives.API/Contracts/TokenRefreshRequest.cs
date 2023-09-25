namespace WebGLives.API.Contracts;

public class TokenRefreshRequest
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}