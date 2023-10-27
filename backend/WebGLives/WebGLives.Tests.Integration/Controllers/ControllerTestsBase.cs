using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using WebGLives.DataAccess;

namespace WebGLives.Tests.Integration.Controllers;

public class ControllerTestsBase : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer;
    protected HttpClient Client;

    protected ControllerTestsBase()
    {
        Environment.SetEnvironmentVariable("DOCKER_HOST", "http://localhost:6770");
        _postgresContainer = new PostgreSqlBuilder().Build();
    }
    
    public virtual async Task InitializeAsync()
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

        Client = appFactory.CreateClient();
    }

    public virtual async Task DisposeAsync() =>
        await _postgresContainer.DisposeAsync();
}