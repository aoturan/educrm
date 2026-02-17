using EduCrm.Modules.People.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Modules.People.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPeopleApplication(this IServiceCollection services)
    {
        return services;
    }   
}