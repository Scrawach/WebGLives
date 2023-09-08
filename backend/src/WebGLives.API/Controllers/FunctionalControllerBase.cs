using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using WebGLives.Core.Errors;

namespace WebGLives.API.Controllers;

public abstract class FunctionalControllerBase : ControllerBase
{
    public IActionResult ResponseFrom<TResult, TError>(Result<TResult, TError> result) where TError : Error =>
        result.IsSuccess
            ? Ok(result.Value)
            : ResponseFrom(result.Error);

    public IActionResult ResponseFrom<TError>(UnitResult<TError> result) where TError : Error =>
        result.IsSuccess
            ? Ok()
            : ResponseFrom(result.Error);

    public IActionResult ResponseFrom<TError>(TError error) where TError : Error =>
        error switch
        {
            NotFoundError notFound => NotFound(notFound.Message),
            _ => BadRequest(error.Message)
        };
}
