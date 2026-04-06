using EduCrm.Modules.People.Application.Repositories;
using EduCrm.Modules.People.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Modules.People.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPeopleInfra(this IServiceCollection services)
    {
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IFollowUpRepository, FollowUpRepository>();
        return services;
    }
}