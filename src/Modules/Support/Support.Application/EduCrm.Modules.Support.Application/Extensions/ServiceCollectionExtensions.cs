using EduCrm.Modules.Support.Application.UseCases.CreateSupportContactMessage;
using EduCrm.Modules.Support.Application.UseCases.CreateSupportRequest;
using EduCrm.Modules.Support.Application.UseCases.ListSupportContactMessages;
using EduCrm.Modules.Support.Application.UseCases.ListSupportRequests;
using EduCrm.Modules.Support.Application.UseCases.MarkSupportContactMessageHandled;
using EduCrm.Modules.Support.Application.UseCases.MarkSupportRequestHandled;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Modules.Support.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSupportApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreateSupportRequestService, CreateSupportRequestService>();
        services.AddScoped<ICreateSupportContactMessageService, CreateSupportContactMessageService>();
        services.AddScoped<IListSupportContactMessagesService, ListSupportContactMessagesService>();
        services.AddScoped<IMarkSupportContactMessageHandledService, MarkSupportContactMessageHandledService>();
        services.AddScoped<IListSupportRequestsService, ListSupportRequestsService>();
        services.AddScoped<IMarkSupportRequestHandledService, MarkSupportRequestHandledService>();
        return services;
    }
}
