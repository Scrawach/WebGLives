using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using WebGLives.API;
using WebGLives.API.Contracts.Games;
using WebGLives.Tests.Integration.Extensions;

namespace WebGLives.Tests.Integration.Controllers;

public class GamesUpdateControllerTests : ControllerTestsBase
{
    [Fact]
    public async Task WhenCreateGame_ThenShouldReturnOkStatus()
    {
        var user = await Client.CreateTestUser();
        var response = await Client.CreateGame(user.AccessToken);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task WhenCreateGame_ThenShouldCreateEmptyGame()
    {
        var user = await Client.CreateTestUser();
        var response = await Client.CreateGame(user.AccessToken);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var game = await Client.GetFromJsonAsync<GameResponse>($"{ApiRouting.Games}/1");
        game.Should().NotBeNull();
        game!.Title.Should().BeNull();
        game.Description.Should().BeNull();
        game.GameUrl.Should().BeNull();
        game.PosterUrl.Should().BeNull();
    }

    [Fact]
    public async Task WhenCreateGame_ThenShouldReceiveUserIdCreatedThisGame()
    {
        var user = await Client.CreateTestUser();
        var response = await Client.CreateGame(user.AccessToken);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var game = await Client.GetFromJsonAsync<GameResponse>($"{ApiRouting.Games}/1");
        game.Should().NotBeNull();
        game!.UserId.Should().Be(1);
    }

    [Fact]
    public async Task WhenCreateGame_AndNotAuthorized_ThenShouldReturnUnauthorizedResponse()
    {
        var createdResponse = await Client.PostAsync(ApiRouting.Games, null);
        createdResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task WhenUpdateTitle_ThenShouldChangeGameTitle()
    {
        const string updatedTitle = "Test";
        
        var user = await Client.CreateTestUser();
        var gameCreatedResponse = await Client.CreateGame(user.AccessToken);
        gameCreatedResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updateResponse = await Client.PutAsync($"{ApiRouting.Games}/1/title", JsonContent.Create(updatedTitle));
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var game = await Client.GetFromJsonAsync<GameResponse>($"{ApiRouting.Games}/1");
        game.Should().NotBeNull();
        game!.Title.Should().NotBeNull();
        game.Title.Should().Be(updatedTitle);
    }

    [Fact]
    public async Task WhenUpdateDescription_ThenShouldChangeGameDescription()
    {
        const string updatedDescription = "Test";
        
        var user = await Client.CreateTestUser();
        var gameCreatedResponse = await Client.CreateGame(user.AccessToken);
        gameCreatedResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updateResponse = await Client.PutAsync($"{ApiRouting.Games}/1/description", JsonContent.Create(updatedDescription));
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var game = await Client.GetFromJsonAsync<GameResponse>($"{ApiRouting.Games}/1");
        game.Should().NotBeNull();
        game!.Description.Should().NotBeNull();
        game.Description.Should().Be(updatedDescription);
    }
}