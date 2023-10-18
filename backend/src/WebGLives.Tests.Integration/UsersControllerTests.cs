using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using WebGLives.API.Contracts.Auth;
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

    [Fact]
    public async Task WhenCreateUser_ThenShouldReturnOkStatus()
    {
        var request = CreateUserRequest("test", "test123");
        var response = await _client.PostAsync("users", request);
        response.EnsureSuccessStatusCode();
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

    private FormUrlEncodedContent CreateUserRequest(string login, string password) =>
        new(new[]
        {
            new KeyValuePair<string, string>("Login", login),
            new KeyValuePair<string, string>("Password", password)
        });
}