using System.Text;
using FluentAssertions;
using JWT.Algorithms;
using Moq;
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
        var accessToken = service.GenerateAccessToken(DateTime.Now);
        
        // assert
        accessToken.Should().NotBeEmpty();
    }

    [Fact]
    public void WhenGenerateAccessToken_ThenShouldSignThisTokenByAlgorithm()
    {
        // arrange
        const string secret = "secret";
        var mocked = Mock.Of<IJwtAlgorithm>(mock => mock.Sign(It.IsAny<byte[]>(), It.IsAny<byte[]>()) == new byte[] { 1, 2, 3, 4, 5 });
        var service = new JwtTokenService(secret, mocked);

        // act
        var accessToken = service.GenerateAccessToken(DateTime.Now);

        // assert
        accessToken.Should().NotBeEmpty();
        Mock.Get(mocked).Verify(mock => mock.Sign(It.IsAny<byte[]>(), It.IsAny<byte[]>()), Times.Once);
    }

    [Fact]
    public void WhenGenerateAccessToken_ThenShouldUsedSecretKeyForAlgorithm()
    {
        // arrange
        const string secret = "secret";
        var mocked = Mock.Of<IJwtAlgorithm>(mock => mock.Sign(It.IsAny<byte[]>(), It.IsAny<byte[]>()) == new byte[] { 1, 2, 3, 4, 5 });
        var service = new JwtTokenService(secret, mocked);
        
        // act
        var accessToken = service.GenerateAccessToken(DateTime.Now);

        // assert
        accessToken.Should().NotBeEmpty();
        Mock.Get(mocked)
            .Verify(mock => mock.Sign
            (
                It.Is<byte[]>(bytes => bytes.SequenceEqual(Encoding.ASCII.GetBytes(secret))), 
                It.IsAny<byte[]>()
            ), Times.Once);
    }

    [Fact]
    public async Task WhenGenerateAccessTokens_WithDelayMoreThanSeconds_ThenShouldReturnUniqueAccessTokens()
    {
        // arrange
        const int millisecondsInSecond = 1000;
        var service = CreateService();

        // act
        var firstAccessToken = service.GenerateAccessToken(DateTime.Now);
        await Task.Delay(millisecondsInSecond);
        var secondAccessToken = service.GenerateAccessToken(DateTime.Now);

        // assert
        firstAccessToken.Should().NotBeEmpty();
        secondAccessToken.Should().NotBeEmpty();
        firstAccessToken.Should().NotBe(secondAccessToken);
    }

    [Fact]
    public void WhenGenerateRefreshToken_ThenShouldReturnUniqueToken()
    {
        // arrange
        var service = CreateService();

        // act
        var refreshToken = service.GenerateRefreshToken();

        // assert
        refreshToken.Should().NotBeEmpty();
    }

    [Fact]
    public void WhenGenerateSeveralRefreshTokenInTime_ThenShouldBeUniques()
    {
        // arrange
        var service = CreateService();

        // act
        var first = service.GenerateRefreshToken();
        var second = service.GenerateRefreshToken();

        // assert
        first.Should().NotBeEmpty();
        second.Should().NotBeEmpty();
        first.Should().NotBe(second);
    }
}