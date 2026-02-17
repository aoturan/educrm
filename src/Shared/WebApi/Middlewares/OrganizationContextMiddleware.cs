using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EduCrm.Infrastructure.Tenancy;
using EduCrm.Modules.Account.Application.Repositories;
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
        IUserOrganizationResolver orgResolver)
    {
        var endpoint = context.GetEndpoint();
        var allowAnonymous = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;

        if (allowAnonymous)
        {
            await _next(context);
            return;
        }

        var isAuthenticated = context.User.Identity?.IsAuthenticated == true;

        if (!isAuthenticated)
        {
            await WriteProblem(context, new[] { CommonErrors.Unauthorized() });
            return;
        }

        var userIdClaim = context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                          ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            await WriteProblem(context, new[] { CommonErrors.Unauthorized() });
            return;
        }

        var orgId = await orgResolver.GetOrganizationIdAsync(userId, context.RequestAborted);

        if (orgId is null)
        {
            await WriteProblem(context, new[] { CommonErrors.Forbidden("Organization scope is missing.") });
            return;
        }

        orgWriter.Set(orgId.Value);
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