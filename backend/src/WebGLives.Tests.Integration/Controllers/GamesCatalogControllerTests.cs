using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WebGLives.API;
using WebGLives.API.Contracts.Games;
using WebGLives.Tests.Integration.Extensions;

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
        var tokens = await Client.CreateTestUser();
        await Client.CreateGame(tokens.AccessToken);

        var response = await Client.GetAsync($"{ApiRouting.Games}/1");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var gameResponse = await response.Content.ReadFromJsonAsync<GameResponse>();
        gameResponse.Should().NotBeNull();
    }

    [Theory]
    [InlineData(5)]
    public async Task WhenGetAllGames_ThenShouldReturnGameResponses_ForEveryGame(int amountOfGames)
    {
        var tokens = await Client.CreateTestUser();
        for (var i = 0; i < amountOfGames; i++) 
            await Client.CreateGame(tokens.AccessToken);

        var response = await Client.GetAsync($"{ApiRouting.Games}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var gameResponses = await response.Content.ReadFromJsonAsync<GameResponse[]>();
        gameResponses.Should().NotBeNull();
        gameResponses!.Length.Should().Be(amountOfGames);
    }
}