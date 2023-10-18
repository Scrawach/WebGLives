using System.Net;
using FluentAssertions;

namespace WebGLives.Tests.Integration.Controllers;

public class UsersControllerTests : ControllerTestsBase
{
    [Theory]
    [InlineData("test", "test123")]
    public async Task WhenCreateUser_ThenShouldReturnOkStatus(string login, string password)
    {
        var response = await Post(CreateUserRequest(login, password));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("test", "")]
    [InlineData("test", "test")]
    [InlineData("test", "test1")]
    public async Task WhenCreateUser_WithPasswordLessThan6Symbols_ThenShouldReturnBadRequest(string login, string password)
    {
        var response = await Post(CreateUserRequest(login, password));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("", "test123")]
    public async Task WhenCreateUser_WithEmptyLogin_ThenShouldReturnBadRequest(string login, string password)
    {
        var response = await Post(CreateUserRequest(login, password));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(1)]
    public async Task WhenGetNotExistingUser_ThenShouldReturnNotFoundRequest(int id)
    {
        var response = await Get(id);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private Task<HttpResponseMessage> Post(HttpContent content) =>
        Client.PostAsync("users", content);

    private Task<HttpResponseMessage> Get(int id) =>
        Client.GetAsync($"users/{id}");

    private static FormUrlEncodedContent CreateUserRequest(string login, string password) =>
        new(new[]
        {
            new KeyValuePair<string, string>("Login", login),
            new KeyValuePair<string, string>("Password", password)
        });
}