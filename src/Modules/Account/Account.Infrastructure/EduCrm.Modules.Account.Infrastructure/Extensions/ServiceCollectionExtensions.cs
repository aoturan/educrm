using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.Security;
using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.Account.Infrastructure.Queries;
using EduCrm.Modules.Account.Infrastructure.Repositories;
using EduCrm.Modules.Account.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Modules.Account.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountInfra(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserOrganizationResolver, UserOrganizationResolver>();
        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IOrganizationReader, OrganizationReader>();
        services.AddScoped<IOrganizationWriter, OrganizationWriter>();
        return services;
    }
}