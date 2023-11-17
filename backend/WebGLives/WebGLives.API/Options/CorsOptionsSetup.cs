using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace WebGLives.API.Options;

public class CorsOptionsSetup : IConfigureOptions<CorsOptions>
{
    public const string CorsName = "Cors";

    private readonly IConfiguration _configuration;

    public CorsOptionsSetup(IConfiguration configuration) =>
        _configuration = configuration;
    
    public void Configure(CorsOptions options)
    {
        var policySetting = new CorsPolicySetting();
        _configuration.GetSection(CorsName).Bind(policySetting);
        options.AddPolicy(name: CorsName, policySetting);
    }
}