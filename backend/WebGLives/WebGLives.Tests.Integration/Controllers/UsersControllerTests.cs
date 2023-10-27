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
        var response = await Client.PostAsync(ApiRouting.Users, RequestBuilder.CreateUserRequest(login, password));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("test", "test123")]
    public async Task WhenCreateUser_ThenShouldReturnUserResponse(string login, string password)
    {
        var createdResponse = await Client.PostAsync(ApiRouting.Users, RequestBuilder.CreateUserRequest(login, password));
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
        var response = await Client.PostAsync(ApiRouting.Users, RequestBuilder.CreateUserRequest(login, password));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("", "test123")]
    public async Task WhenCreateUser_WithEmptyLogin_ThenShouldReturnBadRequest(string login, string password)
    {
        var response = await Client.PostAsync(ApiRouting.Users, RequestBuilder.CreateUserRequest(login, password));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task WhenGetUser_ThenShouldReturnUserResponse()
    {
        const int newUserId = 1;
        const string username = "test";
        const string password = "test123";
        
        var createdResponse = await Client.PostAsync(ApiRouting.Users, RequestBuilder.CreateUserRequest(username, password));
        createdResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await Client.GetFromJsonAsync<UserResponse>($"{ApiRouting.Users}/{newUserId}");

        user.Should().NotBeNull();
        user!.Login.Should().Be(username);
        user.Id.Should().Be(newUserId);
    }

    [Theory]
    [InlineData(1)]
    public async Task WhenGetUser_AndThisUserNotExist_ThenShouldReturnNotFoundResponse(int id)
    {
        var response = await Client.GetAsync($"{ApiRouting.Users}/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}