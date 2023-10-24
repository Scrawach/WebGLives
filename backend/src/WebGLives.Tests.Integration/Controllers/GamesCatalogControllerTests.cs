using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WebGLives.API;
using WebGLives.API.Contracts.Auth;
using WebGLives.API.Contracts.Games;

namespace WebGLives.Tests.Integration.Controllers;

public class GamesCatalogControllerTests : ControllerTestsBase
{
    [Fact]
    public async Task WhenGetGame_AndThisGameNotExits_ThenShouldReturnNotFoundResponse()
    {
        var response = await Client.GetAsync($"{ApiRouting.Games}/1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task WhenGetGame_ThenShouldReturnGameResponse()
    {
        var tokens = await CreateTestUser();
        await CreateGame(tokens.AccessToken);

        var response = await Client.GetAsync($"{ApiRouting.Games}/1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var gameResponse = await response.Content.ReadFromJsonAsync<GameResponse>();
        gameResponse.Should().NotBeNull();
    }

    [Theory]
    [InlineData(5)]
    public async Task WhenGetAllGames_ThenShouldReturnGameResponses_ForEveryGame(int amountOfGames)
    {
        var tokens = await CreateTestUser();
        for (var i = 0; i < amountOfGames; i++) 
            await CreateGame(tokens.AccessToken);

        var response = await Client.GetAsync($"{ApiRouting.Games}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var gameResponses = await response.Content.ReadFromJsonAsync<GameResponse[]>();
        gameResponses.Should().NotBeNull();
        gameResponses!.Length.Should().Be(amountOfGames);
    }

    private async Task<AuthenticatedResponse> CreateTestUser()
    {
        const string login = "test";
        const string password = "test123";
        return await CreateAuthenticatedUser(login, password);
    }

    private async Task CreateGame(string accessToken)
    {
        Client.DefaultRequestHeaders.Authorization = RequestBuilder.BearerAuthenticationHeader(accessToken);
        var createdResponse = await Client.PostAsync(ApiRouting.Games, null);
        createdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task<AuthenticatedResponse> CreateAuthenticatedUser(string login, string password)
    {
        await CreateUser(login, password);
        var tokensResponse = await Client.PostAsync(ApiRouting.Tokens, RequestBuilder.CreateLoginRequest(login, password));
        
        tokensResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var tokens = await tokensResponse.Content.ReadFromJsonAsync<AuthenticatedResponse>();
        tokens.Should().NotBeNull();
        
        return tokens!;
    }
    
    private async Task CreateUser(string login, string password)
    {
        var userCreatedResponse = await Client.PostAsync(ApiRouting.Users, RequestBuilder.CreateUserRequest(login, password));
        userCreatedResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}