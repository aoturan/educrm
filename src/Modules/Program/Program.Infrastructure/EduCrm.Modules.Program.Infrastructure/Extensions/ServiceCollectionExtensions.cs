using EduCrm.Modules.Program.Application.Repositories;
using EduCrm.Modules.Program.Contracts.Abstractions;
using EduCrm.Modules.Program.Infrastructure.Queries;
using EduCrm.Modules.Program.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Modules.Program.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProgramInfra(this IServiceCollection services)
    {
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<IProgramRepository, ProgramRepository>();
        services.AddScoped<IProgramReader, ProgramReader>();
        return services;
    }
}