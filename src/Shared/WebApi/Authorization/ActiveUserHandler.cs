using System.Security.Claims;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace EduCrm.WebApi.Authorization;

public sealed class ActiveUserHandler : AuthorizationHandler<ActiveUserRequirement>
{
    public const string FailureFlagKey = "__active_user_failed";

    private readonly IUserRepository _userRepo;
    private readonly IHttpContextAccessor _httpContext;

    public ActiveUserHandler(IUserRepository userRepo, IHttpContextAccessor httpContext)
    {
        _userRepo = userRepo;
        _httpContext = httpContext;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
    {
        var raw =
            context.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? context.User.FindFirstValue("sub");

        if (!Guid.TryParse(raw, out var userId))
        {
            Flag(context);
            context.Fail();
            return;
        }

        var ct = _httpContext.HttpContext?.RequestAborted ?? CancellationToken.None;
        var user = await _userRepo.GetByIdAsync(userId, ct);

        if (user is null || user.Status != UserStatus.Active)
        {
            Flag(context);
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }

    private void Flag(AuthorizationHandlerContext context)
    {
        var http = _httpContext.HttpContext;
        if (http is not null)
            http.Items[FailureFlagKey] = true;
    }
}
