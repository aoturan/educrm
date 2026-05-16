using EduCrm.Infrastructure.RateLimiting;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.WebApi.Extensions;

public static class RateLimitControllerExtensions
{
    public static async Task<IActionResult?> CheckRateLimitsAsync(
        this ControllerBase controller,
        CancellationToken ct,
        params (string action, string? key)[] checks)
    {
        var limiter = controller.HttpContext.RequestServices.GetRequiredService<IRateLimiter>();

        foreach (var (action, key) in checks)
        {
            if (string.IsNullOrWhiteSpace(key)) continue;

            var decision = await limiter.AcquireAsync(action, key, ct);
            if (!decision.IsAllowed)
                return Result.Fail(CommonErrors.RateLimited()).ToActionResult(controller.HttpContext, controller);
        }

        return null;
    }
}
