using EduCrm.Modules.Program.Application.UseCases.GetApplicationById;
using EduCrm.Modules.Program.Application.UseCases.ArchiveProgram;
using EduCrm.Modules.Program.Application.UseCases.GetDashboardCounts;
using EduCrm.Modules.Program.Application.UseCases.AssignPersonToApplication;
using EduCrm.Modules.Program.Application.UseCases.CloseApplication;
using EduCrm.Modules.Program.Application.UseCases.ContactApplication;
using EduCrm.Modules.Program.Application.UseCases.ConvertApplication;
using EduCrm.Modules.Program.Application.UseCases.Create;
using EduCrm.Modules.Program.Application.UseCases.CreateApplication;
using EduCrm.Modules.Program.Application.UseCases.ListApplications;
using EduCrm.Modules.Program.Application.UseCases.CreateEnrollment;
using EduCrm.Modules.Program.Application.UseCases.ChangeStatus;
using EduCrm.Modules.Program.Application.UseCases.DeleteEnrollment;
using EduCrm.Modules.Program.Application.UseCases.FindPersonsForApplication;
using EduCrm.Modules.Program.Application.UseCases.GetById;
using EduCrm.Modules.Program.Application.UseCases.GetEnrollmentCandidates;
using EduCrm.Modules.Program.Application.UseCases.GetPublicProgramBySlug;
using EduCrm.Modules.Program.Application.UseCases.List;
using EduCrm.Modules.Program.Application.UseCases.ListActive;
using EduCrm.Modules.Program.Application.UseCases.PublishProgram;
using EduCrm.Modules.Program.Application.UseCases.Search;
using EduCrm.Modules.Program.Application.UseCases.UnpublishProgram;
using EduCrm.Modules.Program.Application.UseCases.UpdateProgram;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Modules.Program.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProgramApplication(this IServiceCollection services)
    {
        services.AddScoped<IArchiveProgramService, ArchiveProgramService>();
        services.AddScoped<IGetDashboardCountsService, GetDashboardCountsService>();
        services.AddScoped<IGetApplicationByIdService, GetApplicationByIdService>();
        services.AddScoped<IChangeProgramStatusService, ChangeProgramStatusService>();
        services.AddScoped<ICloseApplicationService, CloseApplicationService>();
        services.AddScoped<IConvertApplicationService, ConvertApplicationService>();
        services.AddScoped<ICreateService, CreateService>();
        services.AddScoped<ICreateApplicationService, CreateApplicationService>();
        services.AddScoped<IAssignPersonToApplicationService, AssignPersonToApplicationService>();
        services.AddScoped<IContactApplicationService, ContactApplicationService>();
        services.AddScoped<IFindPersonsForApplicationService, FindPersonsForApplicationService>();
        services.AddScoped<IListApplicationsService, ListApplicationsService>();
        services.AddScoped<ICreateEnrollmentService, CreateEnrollmentService>();
        services.AddScoped<IDeleteEnrollmentService, DeleteEnrollmentService>();
        services.AddScoped<IGetEnrollmentCandidatesService, GetEnrollmentCandidatesService>();
        services.AddScoped<IGetProgramByIdService, GetProgramByIdService>();
        services.AddScoped<IGetPublicProgramBySlugService, GetPublicProgramBySlugService>();
        services.AddScoped<IListActiveProgramsService, ListActiveProgramsService>();
        services.AddScoped<IListProgramsService, ListProgramsService>();
        services.AddScoped<IPublishProgramService, PublishProgramService>();
        services.AddScoped<ISearchProgramsService, SearchProgramsService>();
        services.AddScoped<IUnpublishProgramService, UnpublishProgramService>();
        services.AddScoped<IUpdateProgramService, UpdateProgramService>();
        return services;
    }   
}