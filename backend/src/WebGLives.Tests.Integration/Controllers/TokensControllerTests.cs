using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WebGLives.API.Contracts.Auth;

namespace WebGLives.Tests.Integration.Controllers;

public class TokensControllerTests : ControllerTestsBase
{
    [Fact]
    public async Task WhenCreateTokens_WithInvalidUser_ThenShouldReturnNotFoundResponse()
    {
        var response = await Client.PostAsync(Api.Tokens, Api.CreateLoginRequest("test", "test123"));
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenCreateTokens_WithInvalidPassword_ThenShouldReturnBadRequest()
    {
        const string login = "test";
        const string password = "test123";

        await CreateUser(login, "123test");
        
        var authenticatedResponse = await Client.PostAsync(Api.Tokens, Api.CreateLoginRequest(login, password));
        authenticatedResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task WhenCreateTokens_ThenShouldReturnAuthenticatedResponse()
    {
        const string login = "test";
        const string password = "test123";

        await CreateUser(login, password);
        
        var authenticatedResponse = await Client.PostAsync(Api.Tokens, Api.CreateLoginRequest(login, password));
        authenticatedResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var tokens = await authenticatedResponse.Content.ReadFromJsonAsync<AuthenticatedResponse>();
        tokens.Should().NotBeNull();
        tokens!.AccessToken.Should().NotBeNull();
        tokens.RefreshToken.Should().NotBeNull();
    }

    [Fact]
    public async Task WhenRefreshTokens_AndUserNotAuthorized_ThenShouldReturnUnauthorizedResponse()
    {
        var response = await Client.PutAsync(Api.Tokens, Api.CreateTokenRefreshRequest("test", "test"));
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private async Task CreateUser(string login, string password)
    {
        var userCreatedResponse = await Client.PostAsync(Api.Users, Api.CreateUserRequest(login, password));
        userCreatedResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}