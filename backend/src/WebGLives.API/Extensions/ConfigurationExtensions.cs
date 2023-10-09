namespace WebGLives.API.Extensions;

public static class ConfigurationExtensions
{
    public static string GetAuthenticationSecretKey(this IConfiguration configuration) =>
        configuration["Authentication:SecretKey"]!;
}