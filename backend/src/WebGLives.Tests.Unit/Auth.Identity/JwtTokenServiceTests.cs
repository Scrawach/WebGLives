using System.Security.Claims;
using FluentAssertions;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using WebGLives.Auth.Identity.Services;

namespace WebGLives.Tests.Unit.Auth.Identity;

public class JwtTokenServiceTests
{
    [Fact]
    public void WhenGenerateAccessToken_ThenShouldReturnUniqueToken()
    {
        // arrange
        const string secret = "no secrets, but you need to finish it to length";
        var algorithm = new HMACSHA256Algorithm();
        var service = new JwtTokenService(secret, algorithm);
        
        // act
        var accessToken = service.GenerateAccessToken();
        
        // assert
        accessToken.Should().NotBeEmpty();
    }

    [Theory]
    [MemberData(nameof(Delays))]
    public async Task WhenGenerateFewAccessToken_ThenShouldReturnUniqueTokens(int delay)
    {
        const string secret = "mysupersecret_secretkey!123";
        var algorithm = new HMACSHA256Algorithm();
        
        var first = JwtBuilder.Create()
            .WithAlgorithm(algorithm)
            .WithSecret(secret)
            .ExpirationTime(DateTime.Now.AddMinutes(10))
            .WithVerifySignature(true)
            .Encode();

        await Task.Delay(delay);

        var second = JwtBuilder.Create()
            .WithAlgorithm(algorithm)
            .WithSecret(secret)
            .ExpirationTime(DateTime.Now.AddMinutes(10))
            .WithVerifySignature(true)
            .Encode();

        first.Should().NotBeEmpty();
        second.Should().NotBeEmpty();
        first.Should().NotBe(second);
    }
    
    [Theory]
    [MemberData(nameof(Delays))]
    public async Task WhenGenerateFewAccessToken_ThenShouldReturnUniqueTokens_UseTicks(int delay)
    {
        const string secret = "mysupersecret_secretkey!123";
        var algorithm = new HMACSHA256Algorithm();
        
        var first = JwtBuilder.Create()
            .WithAlgorithm(algorithm)
            .WithSecret(secret)
            .ExpirationTime(DateTime.Now.AddMinutes(10).Ticks)
            .WithVerifySignature(true)
            .Encode();

        await Task.Delay(delay);

        var second = JwtBuilder.Create()
            .WithAlgorithm(algorithm)
            .WithSecret(secret)
            .ExpirationTime(DateTime.Now.AddMinutes(10).Ticks)
            .WithVerifySignature(true)
            .Encode();

        first.Should().NotBeEmpty();
        second.Should().NotBeEmpty();
        first.Should().NotBe(second);
    }

    public static IEnumerable<object[]> Delays => 
        new[] {1, 10, 50, 100, 250, 400, 500, 1000}
            .Select(number => new object[] { number });
}