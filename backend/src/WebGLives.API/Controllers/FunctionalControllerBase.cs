using System.Security.Claims;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using WebGLives.API.Errors;
using WebGLives.Core.Errors;

namespace WebGLives.API.Controllers;

public abstract class FunctionalControllerBase : ControllerBase
{
    protected string? Username => User.Identity?.Name;

    protected Result<int, Error> UserId =>
        HttpContext.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
            .ToResult(new ClaimNameError())
            .Map(claim => (hasId: int.TryParse(claim.Value, out var userId), userId))
            .Finally(result => result is { IsSuccess: true, Value.hasId: true } 
                ? Result.Success<int, Error>(result.Value.userId) 
                : Result.Failure<int, Error>(new ClaimTypeError()));

    protected async Task<IActionResult> AuthorizedResponseFromAsync<TResult, TError>(Func<int, Task<Result<TResult, TError>>> func) where TError : Error =>
        UserId.IsFailure
            ? ResponseFrom(UserId)
            : await ResponseFromAsync(func.Invoke(UserId.Value));
    
    protected async Task<IActionResult> AuthorizedResponseFromAsync<TError>(Func<int, Task<UnitResult<TError>>> func) where TError : Error =>
        UserId.IsFailure
            ? ResponseFrom(UserId)
            : await ResponseFromAsync(func.Invoke(UserId.Value));

    protected async Task<IActionResult> ResponseFromAsync<TResult, TError>(Task<Result<TResult, TError>> resultTask) where TError : Error =>
        ResponseFrom(await resultTask.ConfigureAwait(Result.Configuration.DefaultConfigureAwait));

    protected async Task<IActionResult> ResponseFromAsync<TError>(Task<UnitResult<TError>> resultTask) where TError : Error =>
        ResponseFrom(await resultTask.ConfigureAwait(Result.Configuration.DefaultConfigureAwait));
    
    protected IActionResult ResponseFrom<TResult, TError>(Result<TResult, TError> result) where TError : Error =>
        result.IsSuccess
            ? Ok(result.Value)
            : ResponseFrom(result.Error);
    
    protected IActionResult ResponseFrom<TError>(UnitResult<TError> result) where TError : Error =>
        result.IsSuccess
            ? Ok()
            : ResponseFrom(result.Error);

    private IActionResult ResponseFrom<TError>(TError error) where TError : Error =>
        error switch
        {
            NotFoundError notFound => NotFound(notFound.Message),
            _ => BadRequest(error.Message)
        };
}
