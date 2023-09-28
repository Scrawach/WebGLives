using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using WebGLives.Core.Errors;

namespace WebGLives.API.Controllers;

public abstract class FunctionalControllerBase : ControllerBase
{
    protected async Task<IActionResult> AsyncResponseFrom<TResult, TError>(Task<Result<TResult, TError>> resultTask) where TError : Error
    {
        var result = await resultTask.ConfigureAwait(Result.Configuration.DefaultConfigureAwait);
        return ResponseFrom(result);
    }
    
    protected async Task<IActionResult> AsyncResponseFrom<TError>(Task<UnitResult<TError>> resultTask) where TError : Error
    {
        var unitResult = await resultTask.ConfigureAwait(Result.Configuration.DefaultConfigureAwait);
        return ResponseFrom(unitResult);
    }

    protected IActionResult ResponseFrom<TResult, TError>(Result<TResult, TError> result) where TError : Error =>
        result.IsSuccess
            ? Ok(result.Value)
            : ResponseFrom(result.Error);
    
    protected IActionResult ResponseFrom<TResult>(Result<TResult> result) =>
        result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);

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
