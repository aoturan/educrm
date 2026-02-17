using System.IdentityModel.Tokens.Jwt;
using System.Text;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Options;
using EduCrm.WebApi.Conventions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace EduCrm.WebApi.Extensions;

public static class AuthServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
    {
        // Disable default claim type mapping (keeps "sub" as "sub" instead of mapping to ClaimTypes.NameIdentifier)
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        
        var jwtSection = config.GetSection("Auth:Jwt");
        
        services.Configure<JwtOptions>(jwtSection);

        var jwt = jwtSection.Get<JwtOptions>() ?? new JwtOptions();

        if (string.IsNullOrWhiteSpace(jwt.Issuer) ||
            string.IsNullOrWhiteSpace(jwt.Audience) ||
            string.IsNullOrWhiteSpace(jwt.SigningKey))
        {
            // Boilerplate guard: fail fast in dev if config is missing.
            throw new InvalidOperationException("Auth:Jwt configuration is missing (Issuer/Audience/SigningKey).");
        }

        var keyBytes = Encoding.UTF8.GetBytes(jwt.SigningKey);
        var signingKey = new SymmetricSecurityKey(keyBytes);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // local dev; OK
                options.SaveToken = false;
                options.MapInboundClaims = false; // Keep original claim names (sub, etc.)

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwt.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwt.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2) // small skew
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        // Prevent default empty/HTML response
                        context.HandleResponse();

                        var errors = new[] { CommonErrors.Unauthorized() };
                        var problem = ProblemDetailsFactory.Create(context.HttpContext, errors);

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/problem+json";

                        await context.Response.WriteAsJsonAsync(problem);
                    },
                    OnForbidden = async context =>
                    {
                        var errors = new[] { CommonErrors.Forbidden() };
                        var problem = ProblemDetailsFactory.Create(context.HttpContext, errors);

                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/problem+json";

                        await context.Response.WriteAsJsonAsync(problem);
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }
}