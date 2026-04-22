using EduCrm.Modules.Support.Application.Repositories;
using EduCrm.Modules.Support.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Modules.Support.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSupportInfra(this IServiceCollection services)
    {
        services.AddScoped<ISupportRequestRepository, SupportRequestRepository>();
        return services;
    }
}