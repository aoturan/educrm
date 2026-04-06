using EduCrm.Modules.People.Application.UseCases.ArchivePerson;
using EduCrm.Modules.People.Application.UseCases.ChangeFollowUpStatus;
using EduCrm.Modules.People.Application.Usecases.Create;
using EduCrm.Modules.People.Application.UseCases.Create;
using EduCrm.Modules.People.Application.UseCases.SnoozeFollowUp;
using EduCrm.Modules.People.Application.UseCases.RescheduleDueDate;
using EduCrm.Modules.People.Application.UseCases.CreateFollowUp;
using EduCrm.Modules.People.Application.UseCases.GetById;
using EduCrm.Modules.People.Application.UseCases.GetFollowUpById;
using EduCrm.Modules.People.Application.UseCases.ListFollowUps;
using EduCrm.Modules.People.Application.UseCases.ListPersons;
using EduCrm.Modules.People.Application.UseCases.UpdateFollowUp;
using EduCrm.Modules.People.Application.UseCases.UpdatePerson;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Modules.People.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPeopleApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreatePersonService, CreatePersonService>();
        services.AddScoped<IArchivePersonService, ArchivePersonService>();
        services.AddScoped<IUpdatePersonService, UpdatePersonService>();
        services.AddScoped<ICreateFollowUpService, CreateFollowUpService>();
        services.AddScoped<IChangeFollowUpStatusService, ChangeFollowUpStatusService>();
        services.AddScoped<ISnoozeFollowUpService, SnoozeFollowUpService>();
        services.AddScoped<IRescheduleDueDateService, RescheduleDueDateService>();
        services.AddScoped<IGetPersonByIdService, GetPersonByIdService>();
        services.AddScoped<IGetFollowUpByIdService, GetFollowUpByIdService>();
        services.AddScoped<IUpdateFollowUpService, UpdateFollowUpService>();
        services.AddScoped<IListPersonsService, ListPersonsService>();
        services.AddScoped<IListFollowUpsService, ListFollowUpsService>();
        return services;
    }   
}