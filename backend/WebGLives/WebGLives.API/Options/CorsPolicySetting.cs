using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using WebGLives.Core.Errors;

namespace WebGLives.API.Options;

public class CorsPolicySetting
{
    public string[]? Origins { get; set; }
    public string[]? AllowHeaders { get; set; }
    public string[]? AllowMethods { get; set; }
    public bool? AllowCredentials { get; set; }

    public void Configure(CorsPolicyBuilder policy) =>
        policy
            .ToResult(new Error("To Result Cast Error"))
            .TapIf(Origins is not null, () => policy.WithOrigins(Origins!))
            .TapIf(AllowHeaders is not null, () => policy.WithHeaders(AllowHeaders!))
            .TapIf(AllowMethods is not null, () => policy.WithMethods(AllowMethods!))
            .TapIf(AllowCredentials is not null, () => policy.AllowCredentials());

    public static implicit operator Action<CorsPolicyBuilder>(CorsPolicySetting setting) =>
        setting.Configure;
}