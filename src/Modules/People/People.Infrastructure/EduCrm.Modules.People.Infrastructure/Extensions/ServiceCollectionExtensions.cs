using EduCrm.Modules.People.Application.Repositories;
using EduCrm.Modules.People.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Modules.People.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPeopleInfra(this IServiceCollection services)
    {
        // TODO (Phase 1): register People module services & repositories here.
        // Example:
        // services.AddScoped<IPeopleService, PeopleService>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        

        return services;
    }
}