using System.Security.Claims;
using EduCrm.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Http;

namespace EduCrm.Infrastructure.Auth;

public sealed class HttpCurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _http;

    public HttpCurrentUser(IHttpContextAccessor http)
    {
        _http = http;
    }

    public bool IsAuthenticated =>
        _http.HttpContext?.User?.Identity?.IsAuthenticated == true;

    public Guid? UserId
    {
        get
        {
            var user = _http.HttpContext?.User;
            if (user is null) return null;

            var raw =
                user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirstValue("sub");

            return Guid.TryParse(raw, out var id) ? id : null;
        }
    }
}