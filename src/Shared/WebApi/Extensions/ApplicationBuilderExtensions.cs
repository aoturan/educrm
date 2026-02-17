using EduCrm.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace EduCrm.WebApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobalMiddlewares(this IApplicationBuilder app)
    {
        // Exception handling should wrap everything.
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
}