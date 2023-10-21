using System.Net;
using FluentAssertions;

namespace WebGLives.Tests.Integration.Controllers;

public class GamesCatalogControllerTests : ControllerTestsBase
{
    [Fact]
    public async Task WhenGetGame_AndThisGameNotExits_ThenShouldReturnNotFoundResponse()
    {
        var response = await Client.GetAsync($"{Api.GamesCatalog}/0");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}