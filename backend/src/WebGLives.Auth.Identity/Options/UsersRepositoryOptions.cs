namespace WebGLives.Auth.Identity.Options;

public class UsersRepositoryOptions
{
    public const string UsersRepository = "UsersRepository";
    
    public string LocalProvider { get; set; } = string.Empty;
    public string RefreshTokenTable { get; set; } = string.Empty;
}