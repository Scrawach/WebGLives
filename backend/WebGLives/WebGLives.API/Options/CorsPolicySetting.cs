using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using WebGLives.Core.Errors;

namespace WebGLives.API.Options;

public class CorsPolicySetting
{
    public string[]? Origins { get; set; }
    public bool? AllowAnyMethods { get; set; }
    public bool? AllowAnyHeaders { get; set; }
    public bool? AllowCredentials { get; set; }

    public void Configure(CorsPolicyBuilder policy) =>
        policy
            .ToResult(new Error("To Result Cast Error"))
            .TapIf(Origins is not null, () => policy.WithOrigins(Origins!))
            .TapIf(AllowAnyMethods is not null, () => policy.AllowAnyMethod())
            .TapIf(AllowAnyHeaders is not null, () => policy.AllowAnyHeader())
            .TapIf(AllowCredentials is not null, () => policy.AllowCredentials());

    public static implicit operator Action<CorsPolicyBuilder>(CorsPolicySetting setting) =>
        setting.Configure;
}