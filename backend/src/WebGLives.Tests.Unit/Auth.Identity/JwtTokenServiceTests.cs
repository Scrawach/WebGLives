using System.Security.Claims;
using FluentAssertions;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using WebGLives.Auth.Identity.Services;

namespace WebGLives.Tests.Unit.Auth.Identity;

public class JwtTokenServiceTests
{
    private static JwtTokenService CreateService()
    {
        const string secret = "no secrets, but you need to finish it to length";
        var algorithm = new HMACSHA256Algorithm();
        return new JwtTokenService(secret, algorithm);
    }
    
    [Fact]
    public void WhenGenerateAccessToken_ThenShouldReturnUniqueToken()
    {
        // arrange
        var service = CreateService();
        
        // act
        var accessToken = service.GenerateAccessToken();
        
        // assert
        accessToken.Should().NotBeEmpty();
    }

    [Fact]
    public async Task WhenGenerateAccessTokens_WithDelayMoreThanSeconds_ThenShouldReturnUniqueAccessTokens()
    {
        // arrange
        const int millisecondsInSecond = 1000;
        var service = CreateService();

        // act
        var firstAccessToken = service.GenerateAccessToken();
        await Task.Delay(millisecondsInSecond);
        var secondAccessToken = service.GenerateAccessToken();

        // assert
        firstAccessToken.Should().NotBeEmpty();
        secondAccessToken.Should().NotBeEmpty();
        firstAccessToken.Should().NotBe(secondAccessToken);
    }
}