using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WebGLives.API;
using WebGLives.API.Contracts.Auth;
using WebGLives.Tests.Integration.Extensions;

namespace WebGLives.Tests.Integration.Controllers;

public class TokensControllerTests : ControllerTestsBase
{
    [Fact]
    public async Task WhenCreateTokens_WithInvalidUser_ThenShouldReturnNotFoundResponse()
    {
        var response = await Client.PostAsync(ApiRouting.Tokens, RequestBuilder.CreateLoginRequest("test", "test123"));
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenCreateTokens_WithInvalidPassword_ThenShouldReturnBadRequest()
    {
        const string login = "test";
        const string password = "test123";

        await Client.CreateUser(login, "123test");
        
        var authenticatedResponse = await Client.PostAsync(ApiRouting.Tokens, RequestBuilder.CreateLoginRequest(login, password));
        authenticatedResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task WhenCreateTokens_ThenShouldReturnAuthenticatedResponse()
    {
        const string login = "test";
        const string password = "test123";

        await Client.CreateUser(login, password);
        
        var authenticatedResponse = await Client.PostAsync(ApiRouting.Tokens, RequestBuilder.CreateLoginRequest(login, password));
        authenticatedResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var tokens = await authenticatedResponse.Content.ReadFromJsonAsync<AuthenticatedResponse>();
        tokens.Should().NotBeNull();
        tokens!.AccessToken.Should().NotBeNull();
        tokens.RefreshToken.Should().NotBeNull();
    }

    [Fact]
    public async Task WhenRefreshTokens_AfterDelayMoreThanOneSecond_ThenShouldReturnDifferentTokens()
    {
        const string login = "test";
        const string password = "test123";
        const int millisecondsInSecond = 1000; // one second is minimal value for time offset between generation, for jwt token

        var tokens = await Client.CreateAuthenticatedUser(login, password);
        
        Client.DefaultRequestHeaders.Authorization = RequestBuilder.BearerAuthenticationHeader(tokens!.AccessToken);
        await Task.Delay(millisecondsInSecond);
        var refreshResponse = await Client.PutAsync(ApiRouting.Tokens, RequestBuilder.CreateTokenRefreshRequest(tokens.AccessToken, tokens.RefreshToken));
        refreshResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var refreshedTokens = await refreshResponse.Content.ReadFromJsonAsync<AuthenticatedResponse>();
        refreshedTokens.Should().NotBeNull();
        refreshedTokens!.AccessToken.Should().NotBe(tokens.AccessToken);
        refreshedTokens.RefreshToken.Should().NotBe(tokens.RefreshToken);
    }

    [Fact]
    public async Task WhenRefreshTokens_AndUserNotAuthorized_ThenShouldReturnUnauthorizedResponse()
    {
        var response = await Client.PutAsync(ApiRouting.Tokens, RequestBuilder.CreateTokenRefreshRequest("test", "test"));
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}