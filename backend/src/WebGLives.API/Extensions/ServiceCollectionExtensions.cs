using JWT.Algorithms;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using WebGLives.Auth.Identity.Repositories;
using WebGLives.Auth.Identity.Services;
using WebGLives.BusinessLogic.Services;
using WebGLives.BusinessLogic.Services.Abstract;
using WebGLives.Core.Repositories;
using WebGLives.DataAccess;
using WebGLives.DataAccess.Repositories;

namespace WebGLives.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebGLives(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.Seq(builder.Configuration.GetConnectionString(nameof(Serilog))!)
            .CreateLogger();
        
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(logger, dispose: true);
        });
        
        services.AddDbContext<GamesDbContext>(options =>
        {
            options.UseNpgsql
            (
                builder.Configuration.GetConnectionString(nameof(GamesDbContext)),
                npgsqlOptions => npgsqlOptions.MigrationsAssembly("WebGLives.Migrations")
            );
        });
        
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<DataAccessMappingProfile>();
        });

        services.AddSingleton<IZipService, ZipService>();
        services.AddSingleton<IFilesService, FilesService>();
        services.AddSingleton<IJwtAlgorithm, HMACSHA256Algorithm>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>(provider =>
        {
            var secretKey = builder.Configuration.GetAuthenticationSecretKey();
            return new JwtTokenService(secretKey, provider.GetService<IJwtAlgorithm>()!);
        });
        
        services.AddScoped<ITokenFactory, TokenFactory>();
        services.AddScoped<IGamesCatalogService, GamesService>();
        services.AddScoped<IGamesUpdateService, GamesService>();
        services.AddScoped<IGamesRepository, GamesRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        return services;
    }

    public static IServiceCollection AddSwaggerWithAuthentication(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme()
            {
                BearerFormat = "JWT",
                Description = @"Put **_ONLY_** your JWT Bearer token on textbox below!",
                Name = "Bearer",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Reference = new OpenApiReference()
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
    
            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
        });
        return services;
    }
}