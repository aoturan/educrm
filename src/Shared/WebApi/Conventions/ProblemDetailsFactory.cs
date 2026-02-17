using System.Diagnostics;
using EduCrm.SharedKernel.Errors;
using Microsoft.AspNetCore.Mvc;

namespace EduCrm.WebApi.Conventions;

public static class ProblemDetailsFactory
{
    public static ProblemDetails Create(HttpContext httpContext, IReadOnlyList<Error> errors)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        var (statusCode, title) = MapStatusAndTitle(errors);

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = errors.Count == 1 ? errors[0].Message : "One or more errors occurred.",
            Instance = httpContext.Request.Path
        };

        // Extensions
        problem.Extensions["traceId"] = traceId;
        problem.Extensions["errors"] = errors.Select(ToSerializableError).ToArray();

        return problem;
    }

    private static (int statusCode, string title) MapStatusAndTitle(IReadOnlyList<Error> errors)
    {
        // If there are multiple errors, prefer the "most severe" (by HTTP status).
        var codes = errors.Select(e => ErrorHttpStatusMapper.GetStatusCode(e.Code)).ToArray();
        var status = codes.Max();

        return status switch
        {
            StatusCodes.Status400BadRequest => (status, "Bad Request"),
            StatusCodes.Status401Unauthorized => (status, "Unauthorized"),
            StatusCodes.Status403Forbidden => (status, "Forbidden"),
            StatusCodes.Status404NotFound => (status, "Not Found"),
            StatusCodes.Status409Conflict => (status, "Conflict"),
            _ => (status, "Error")
        };
    }


    private static object ToSerializableError(Error error) => new
    {
        code = error.Code,
        message = error.Message,
        metadata = error.Metadata
    };
}
