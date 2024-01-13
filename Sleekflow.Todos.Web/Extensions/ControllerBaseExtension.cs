using Microsoft.AspNetCore.Mvc;
using Sleekflow.Todos.Core.Models;

namespace Sleekflow.Todos.Web.Extensions;

public static class ControllerBaseExtension
{
    public static IActionResult ToReponse<T>(
        this ControllerBase controllerBase,
        ServiceResponseModel<T> result
    )
    {
        return MapToResult<T>(controllerBase, result);
    }

    public static IActionResult ToReponse(
        this ControllerBase controllerBase,
        ServiceResponseModel result
    )
    {
        return MapToResult(controllerBase, result);
    }

    private static IActionResult MapToResult<T>(
        this ControllerBase controllerBase,
        ServiceResponseModel<T> result
    )
    {
        if (result.Errors.Count > 1)
        {
            return controllerBase.BadRequest(result.Errors);
        }

        if (result.Errors.Count == 1)
        {
            if (typeof(NotFoundError) == result.Errors[0].GetType())
            {
                return controllerBase.NotFound(result.Errors);
            }

            return controllerBase.BadRequest(result.Errors);
        }

        return controllerBase.Ok(result.Data);
    }

    private static IActionResult MapToResult(
        this ControllerBase controllerBase,
        ServiceResponseModel result
    )
    {
        if (result.Errors.Count > 1)
        {
            return controllerBase.BadRequest(result.Errors);
        }

        if (result.Errors.Count == 1)
        {
            if (typeof(NotFoundError) == result.Errors[0].GetType())
            {
                return controllerBase.NotFound(result.Errors);
            }

            return controllerBase.BadRequest(result.Errors);
        }

        return controllerBase.Ok();
    }
}