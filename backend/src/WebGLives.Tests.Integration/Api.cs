namespace WebGLives.Tests.Integration;

public static class Api
{
    public const string Users = "users";
    public const string Tokens = "tokens";

    public static FormUrlEncodedContent CreateUserRequest(string login, string password) =>
        new(new[]
        {
            new KeyValuePair<string, string>("Login", login),
            new KeyValuePair<string, string>("Password", password)
        });

    public static FormUrlEncodedContent CreateLoginRequest(string login, string password) =>
        CreateUserRequest(login, password);

    public static FormUrlEncodedContent CreateTokenRefreshRequest(string access, string refresh) =>
        new(new[]
        {
            new KeyValuePair<string, string>("Access", access),
            new KeyValuePair<string, string>("Refresh", refresh)
        });
}