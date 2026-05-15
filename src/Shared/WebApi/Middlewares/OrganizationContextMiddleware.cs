using EduCrm.Infrastructure.Tenancy;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.WebApi.Conventions;
using Microsoft.AspNetCore.Authorization;

namespace EduCrm.WebApi.Middlewares;

public sealed class OrganizationContextMiddleware
{
    private readonly RequestDelegate _next;

    public OrganizationContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IOrgContextWriter orgWriter,
        ICurrentUserSnapshot snapshot)
    {
        var endpoint = context.GetEndpoint();
        var allowAnonymous = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;

        if (allowAnonymous)
        {
            await _next(context);
            return;
        }

        if (!snapshot.IsLoaded)
        {
            await WriteProblem(context, new[] { CommonErrors.Unauthorized() });
            return;
        }

        orgWriter.Set(snapshot.OrganizationId);
        await _next(context);
    }

    private static async Task WriteProblem(HttpContext context, IReadOnlyList<Error> errors)
    {
        var problem = ProblemDetailsFactory.Create(context, errors);

        context.Response.StatusCode = problem.Status ?? StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problem);
    }
}
