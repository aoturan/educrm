using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EduCrm.Infrastructure.Auth;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace EduCrm.WebApi.Middlewares;

public sealed class CurrentUserSnapshotMiddleware
{
    private readonly RequestDelegate _next;

    public CurrentUserSnapshotMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        CurrentUserSnapshot snapshot,
        IUserRepository userRepo)
    {
        var endpoint = context.GetEndpoint();
        var allowAnonymous = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null;

        if (allowAnonymous)
        {
            await _next(context);
            return;
        }

        if (context.User.Identity?.IsAuthenticated != true)
        {
            await _next(context);
            return;
        }

        var rawUserId = context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                        ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(rawUserId, out var userId))
        {
            await _next(context);
            return;
        }

        var user = await userRepo.GetByIdWithOrganizationAsync(userId, context.RequestAborted);

        if (user is not null)
        {
            snapshot.Set(
                user.Id,
                user.OrganizationId,
                user.OrganizationName,
                user.Email,
                user.FullName,
                user.Status == UserStatus.Active,
                user.IsApplicationAdmin,
                user.Role);
        }

        await _next(context);
    }
}
