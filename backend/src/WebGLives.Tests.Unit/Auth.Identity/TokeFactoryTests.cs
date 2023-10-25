using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;
using WebGLives.Auth.Identity;
using WebGLives.Auth.Identity.Services;
using WebGLives.Core;
using WebGLives.Core.Errors;
using WebGLives.Core.Repositories;

namespace WebGLives.Tests.Unit.Auth.Identity;

public class TokeFactoryTests
{
    [Fact]
    public async Task WhenCreateToken_AndNotFoundUser_ThenShouldReturnTestError()
    {
        // arrange
        const string username = "test";
        const string password = "test123";
        const string errorMessage = "test error";
        
        Result.Success<IUser, Error>(new User(username));

        var mockedUsersRepository = new Mock<IUsersRepository>();
        mockedUsersRepository
            .Setup(mock => mock.FindByNameAsync(It.IsAny<string>()))
            .Returns(() =>  Task.FromResult(Result.Failure<IUser, Error>(new Error(errorMessage))));
        
        var refreshRepository = Mock.Of<IRefreshTokensRepository>();
        var tokenService = Mock.Of<IJwtTokenService>();
        var factory = new TokenFactory(mockedUsersRepository.Object, refreshRepository, tokenService);

        // act
        var result = await factory.Create(username, password);

        // assert
        result.IsFailure.Should().BeTrue();
        result.Error.Message.Should().Be(errorMessage);
    }
}