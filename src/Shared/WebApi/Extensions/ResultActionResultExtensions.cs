using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;
using EduCrm.WebApi.Conventions;
using Microsoft.AspNetCore.Mvc;

namespace EduCrm.WebApi.Extensions;

public static class ResultActionResultExtensions
{
    public static IActionResult ToActionResult(this Result result, HttpContext http, ControllerBase controller)
    {
        if (result.IsSuccess) return controller.NoContent();

        var problem = ProblemDetailsFactory.Create(http, result.Errors);
        return controller.StatusCode(problem.Status ?? StatusCodes.Status400BadRequest, problem);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result, HttpContext http, ControllerBase controller, Func<T, IActionResult> onSuccess)
    {
        if (result.IsSuccess) return onSuccess(result.Value);

        var problem = ProblemDetailsFactory.Create(http, result.Errors);
        return controller.StatusCode(problem.Status ?? StatusCodes.Status400BadRequest, problem);
    }
}