using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using WebGLives.DataAccess;

namespace WebGLives.Tests.Integration;

public class UsersControllerTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;
    private HttpClient _client;

    public UsersControllerTests()
    {
        Environment.SetEnvironmentVariable("DOCKER_HOST", "http://localhost:6770");
        _postgresContainer = new PostgreSqlBuilder().Build();
    }

    [Theory]
    [InlineData("test", "test123")]
    public async Task WhenCreateUser_ThenShouldReturnOkStatus(string login, string password)
    {
        var response = await Post(CreateUserRequest(login, password));
        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData("test", "")]
    [InlineData("test", "test")]
    [InlineData("test", "test1")]
    public async Task WhenCreateUser_AndPasswordLessThan6Symbols_ThenShouldReturnBadRequest(string login, string password)
    {
        var response = await Post(CreateUserRequest(login, password));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    public async Task InitializeAsync()
    {
        const string dbConnectionStringSection = "ConnectionStrings:GamesDbContext";
        
        await _postgresContainer.StartAsync();

        var connectionString = _postgresContainer.GetConnectionString();
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((_, configurationBuilder) =>
                {
                    configurationBuilder.AddInMemoryCollection(new List<KeyValuePair<string, string?>>()
                    {
                        new(dbConnectionStringSection, connectionString)
                    });
                });
            });

        using var scope = appFactory.Services.CreateScope();
        var gamesDbContext = scope.ServiceProvider.GetRequiredService<GamesDbContext>();
        await gamesDbContext.Database.MigrateAsync();

        _client = appFactory.CreateClient();
    }

    public async Task DisposeAsync() =>
        await _postgresContainer.DisposeAsync();

    private Task<HttpResponseMessage> Post(HttpContent content) =>
        _client.PostAsync("users", content);

    private static FormUrlEncodedContent CreateUserRequest(string login, string password) =>
        new(new[]
        {
            new KeyValuePair<string, string>("Login", login),
            new KeyValuePair<string, string>("Password", password)
        });
}