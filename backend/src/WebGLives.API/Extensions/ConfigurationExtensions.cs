namespace WebGLives.API.Extensions;

public static class ConfigurationExtensions
{
    private const string SecretKeyNotFound = "Not found Authentication:SecretKey in app settings";

    public static string GetAuthenticationSecretKey(this IConfiguration configuration) =>
        configuration["Authentication:SecretKey"] ?? throw new Exception(SecretKeyNotFound);
}