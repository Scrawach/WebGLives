namespace WebGLives.Auth.Identity;

public class Tokens
{
    public string Access { get; set; }
    public string Refresh { get; set; }
    public DateTime ExpireAt { get; set; }
}