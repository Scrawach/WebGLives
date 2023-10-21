using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WebGLives.API;
using WebGLives.API.Contracts.Users;

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
    [InlineData("test", "test123")]
    public async Task WhenCreateUser_ThenShouldReturnUserResponse(string login, string password)
    {
        var createdResponse = await Post(CreateUserRequest(login, password));
        var content = await createdResponse.Content.ReadFromJsonAsync<UserResponse>();
        
        createdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().NotBeNull();
        content!.Login.Should().Be(login);
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

    [Fact]
    public async Task WhenGetUser_ThenShouldReturnUserResponse()
    {
        const int newUserId = 1;
        const string username = "test";
        const string password = "test123";
        
        var createdResponse = await Post(CreateUserRequest(username, password));
        createdResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await Client.GetFromJsonAsync<UserResponse>($"{ApiRoutings.Users}/{newUserId}");

        user.Should().NotBeNull();
        user!.Login.Should().Be(username);
        user.Id.Should().Be(newUserId);
    }

    [Theory]
    [InlineData(1)]
    public async Task WhenGetUser_AndThisUserNotExist_ThenShouldReturnNotFoundResponse(int id)
    {
        var response = await Get(id);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private Task<HttpResponseMessage> Post(HttpContent content) =>
        Client.PostAsync(ApiRoutings.Users, content);

    private Task<HttpResponseMessage> Get(int id) =>
        Client.GetAsync($"{ApiRoutings.Users}/{id}");

    private static FormUrlEncodedContent CreateUserRequest(string login, string password) =>
        new(new[]
        {
            new KeyValuePair<string, string>("Login", login),
            new KeyValuePair<string, string>("Password", password)
        });
}