using EduCrm.Modules.Support.Application.UseCases.CreateSupportRequest;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Modules.Support.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSupportApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreateSupportRequestService, CreateSupportRequestService>();
        return services;
    }
}