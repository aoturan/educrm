using EduCrm.Modules.People.Application.Repositories;
using EduCrm.Modules.People.Contracts.Abstractions;
using EduCrm.Modules.People.Infrastructure.Readers;
using EduCrm.Modules.People.Infrastructure.Repositories;
using EduCrm.Modules.People.Infrastructure.Writers;using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Modules.People.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPeopleInfra(this IServiceCollection services)
    {
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IFollowUpRepository, FollowUpRepository>();
        services.AddScoped<IPersonWriter, PersonWriter>();
        services.AddScoped<IPersonReader, PersonReader>();
        services.AddScoped<IFollowUpReader, FollowUpReader>();
        return services;
    }
}