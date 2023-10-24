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
    public async Task WhenCreateGame_AndNotAuthorized_ThenShouldReturnUnauthorizedResponse()
    {
        var createdResponse = await Client.PostAsync(ApiRouting.Games, null);
        createdResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task WhenUpdateTitle_ThenShouldChangeTitle_AndReturnOkStatus()
    {
        const string newTitle = "Test";
        
        var user = await Client.CreateTestUser();
        var gameCreatedResponse = await Client.CreateGame(user.AccessToken);
        gameCreatedResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updateResponse = await Client.PutAsync($"{ApiRouting.Games}/1/title", JsonContent.Create(newTitle));
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var game = await Client.GetFromJsonAsync<GameResponse>($"{ApiRouting.Games}/1");
        game.Should().NotBeNull();
        game!.Title.Should().NotBeNull();
        game.Title.Should().Be(newTitle);
    }
}