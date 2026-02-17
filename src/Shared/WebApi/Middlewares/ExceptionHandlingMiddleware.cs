using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EduCrm.WebApi.Middlewares;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            // Best-effort context (may be null if middleware order/config changes)
            var userId = context.User?.FindFirst("sub")?.Value
                         ?? context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var orgId = context.User?.FindFirst("org_id")?.Value;

            _logger.LogError(ex,
                "Unhandled exception. traceId={TraceId} orgId={OrgId} userId={UserId} method={Method} path={Path}",
                traceId, orgId, userId, context.Request.Method, context.Request.Path.Value);

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred.",
                Instance = context.Request.Path
            };

            problem.Extensions["traceId"] = traceId;

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}