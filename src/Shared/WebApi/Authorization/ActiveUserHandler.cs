using EduCrm.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace EduCrm.WebApi.Authorization;

public sealed class ActiveUserHandler : AuthorizationHandler<ActiveUserRequirement>
{
    public const string FailureFlagKey = "__active_user_failed";

    private readonly ICurrentUserSnapshot _snapshot;
    private readonly IHttpContextAccessor _httpContext;

    public ActiveUserHandler(ICurrentUserSnapshot snapshot, IHttpContextAccessor httpContext)
    {
        _snapshot = snapshot;
        _httpContext = httpContext;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ActiveUserRequirement requirement)
    {
        if (!_snapshot.IsLoaded || !_snapshot.IsActive)
        {
            Flag();
            context.Fail();
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private void Flag()
    {
        var http = _httpContext.HttpContext;
        if (http is not null)
            http.Items[FailureFlagKey] = true;
    }
}
