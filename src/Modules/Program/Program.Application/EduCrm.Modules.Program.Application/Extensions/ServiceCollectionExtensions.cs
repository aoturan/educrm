using EduCrm.Modules.Program.Application.UseCases.ArchiveProgram;
using EduCrm.Modules.Program.Application.UseCases.Create;
using EduCrm.Modules.Program.Application.UseCases.CreateEnrollment;
using EduCrm.Modules.Program.Application.UseCases.ChangeStatus;
using EduCrm.Modules.Program.Application.UseCases.DeleteEnrollment;
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
        services.AddScoped<IChangeProgramStatusService, ChangeProgramStatusService>();
        services.AddScoped<ICreateService, CreateService>();
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