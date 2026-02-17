using EduCrm.Modules.Account.Application.UseCases.ChangePassword;
using EduCrm.Modules.Account.Application.UseCases.Fail;
using EduCrm.Modules.Account.Application.UseCases.GetMe;
using EduCrm.Modules.Account.Application.UseCases.Login;
using EduCrm.Modules.Account.Application.UseCases.Register;
using EduCrm.Modules.Account.Application.UseCases.UpdateProfile;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Modules.Account.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountApplication(this IServiceCollection services)
    {
        services.AddScoped<IRegisterService, RegisterService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IGetMeService, GetMeService>();
        services.AddScoped<IChangePasswordService, ChangePasswordService>();
        services.AddScoped<IUpdateProfileService, UpdateProfileService>();
        services.AddScoped<IFailService, FailService>();
        return services;
    }   
}