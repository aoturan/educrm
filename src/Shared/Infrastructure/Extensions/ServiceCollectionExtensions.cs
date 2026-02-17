using EduCrm.Infrastructure.Auth;
using EduCrm.Infrastructure.Data;
using EduCrm.Infrastructure.Persistence;
using EduCrm.Infrastructure.Tenancy;
using EduCrm.Infrastructure.Time;
using EduCrm.SharedKernel.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddSingleton<IClock, SystemClock>();

        // Request-scoped org context (read via IOrgContext, set via IOrgContextWriter)
        services.AddScoped<OrgContext>();
        services.AddScoped<IOrgContext>(sp => sp.GetRequiredService<OrgContext>());
        services.AddScoped<IOrgContextWriter>(sp => sp.GetRequiredService<OrgContext>());

        services.AddScoped<ICurrentUser, HttpCurrentUser>();
        
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IAppDbTransaction, AppDbTransaction>();

        return services;
    }
}