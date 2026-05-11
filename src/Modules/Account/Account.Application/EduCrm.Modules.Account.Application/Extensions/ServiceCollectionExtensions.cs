using EduCrm.Modules.Account.Application.UseCases.ApproveSubscriptionRequest;
using EduCrm.Modules.Account.Application.UseCases.ChangePassword;
using EduCrm.Modules.Account.Application.UseCases.ChangeUserPasswordByAdmin;
using EduCrm.Modules.Account.Application.UseCases.ChangeUserStatus;
using EduCrm.Modules.Account.Application.UseCases.CreatePaymentNotification;
using EduCrm.Modules.Account.Application.UseCases.CreateSubscriptionRequest;
using EduCrm.Modules.Account.Application.UseCases.CreateUser;
using EduCrm.Modules.Account.Application.UseCases.DownloadPaymentNotificationReceipt;
using EduCrm.Modules.Account.Application.UseCases.Fail;
using EduCrm.Modules.Account.Application.UseCases.GetBankAccounts;
using EduCrm.Modules.Account.Application.UseCases.GetBillingDetail;
using EduCrm.Modules.Account.Application.UseCases.GetAdminDashboard;
using EduCrm.Modules.Account.Application.UseCases.GetMe;
using EduCrm.Modules.Account.Application.UseCases.GetNotifications;
using EduCrm.Modules.Account.Application.UseCases.GetOrganization;
using EduCrm.Modules.Account.Application.UseCases.GetOrganizationOverview;
using EduCrm.Modules.Account.Application.UseCases.GetPlanPricing;
using EduCrm.Modules.Account.Application.UseCases.GetPlanUsage;
using EduCrm.Modules.Account.Application.UseCases.GetSubscriptionRequestDetail;
using EduCrm.Modules.Account.Application.UseCases.ListOrganizations;
using EduCrm.Modules.Account.Application.UseCases.ListSubscriptionRequests;
using EduCrm.Modules.Account.Application.UseCases.ListUsers;
using EduCrm.Modules.Account.Application.UseCases.Login;
using EduCrm.Modules.Account.Application.UseCases.MarkSubscriptionRequestInvoiced;
using EduCrm.Modules.Account.Application.UseCases.OverrideOrganizationSubscription;
using EduCrm.Modules.Account.Application.UseCases.Register;
using EduCrm.Modules.Account.Application.UseCases.RequestPasswordReset;
using EduCrm.Modules.Account.Application.UseCases.ResetPassword;
using EduCrm.Modules.Account.Application.UseCases.TransferAdminRole;
using EduCrm.Modules.Account.Application.UseCases.UpdateOrganization;
using EduCrm.Modules.Account.Application.UseCases.UpdateProfile;
using EduCrm.Modules.Account.Application.UseCases.UpdateUserByAdmin;
using EduCrm.Modules.Account.Application.UseCases.UpsertBillingDetail;
using Microsoft.Extensions.DependencyInjection;

namespace EduCrm.Modules.Account.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountApplication(this IServiceCollection services)
    {
        services.AddScoped<IRegisterService, RegisterService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IGetMeService, GetMeService>();
        services.AddScoped<IGetNotificationsService, GetNotificationsService>();
        services.AddScoped<IGetAdminDashboardService, GetAdminDashboardService>();
        services.AddScoped<IGetOrganizationService, GetOrganizationService>();
        services.AddScoped<IGetOrganizationOverviewService, GetOrganizationOverviewService>();
        services.AddScoped<IUpdateOrganizationService, UpdateOrganizationService>();
        services.AddScoped<IChangePasswordService, ChangePasswordService>();
        services.AddScoped<IRequestPasswordResetService, RequestPasswordResetService>();
        services.AddScoped<IResetPasswordService, ResetPasswordService>();
        services.AddScoped<IUpdateProfileService, UpdateProfileService>();
        services.AddScoped<ICreateUserService, CreateUserService>();
        services.AddScoped<IChangeUserStatusService, ChangeUserStatusService>();
        services.AddScoped<IChangeUserPasswordByAdminService, ChangeUserPasswordByAdminService>();
        services.AddScoped<IListUsersService, ListUsersService>();
        services.AddScoped<IListOrganizationsService, ListOrganizationsService>();
        services.AddScoped<IListSubscriptionRequestsService, ListSubscriptionRequestsService>();
        services.AddScoped<IUpdateUserByAdminService, UpdateUserByAdminService>();
        services.AddScoped<ITransferAdminRoleService, TransferAdminRoleService>();
        services.AddScoped<IGetPlanUsageService, GetPlanUsageService>();
        services.AddScoped<IGetPlanPricingService, GetPlanPricingService>();
        services.AddScoped<IGetBankAccountsService, GetBankAccountsService>();
        services.AddScoped<IGetBillingDetailService, GetBillingDetailService>();
        services.AddScoped<IUpsertBillingDetailService, UpsertBillingDetailService>();
        services.AddScoped<ICreateSubscriptionRequestService, CreateSubscriptionRequestService>();
        services.AddScoped<ICreatePaymentNotificationService, CreatePaymentNotificationService>();
        services.AddScoped<IGetSubscriptionRequestDetailService, GetSubscriptionRequestDetailService>();
        services.AddScoped<IDownloadPaymentNotificationReceiptService, DownloadPaymentNotificationReceiptService>();
        services.AddScoped<IApproveSubscriptionRequestService, ApproveSubscriptionRequestService>();
        services.AddScoped<IMarkSubscriptionRequestInvoicedService, MarkSubscriptionRequestInvoicedService>();
        services.AddScoped<IOverrideOrganizationSubscriptionService, OverrideOrganizationSubscriptionService>();
        services.AddScoped<IFailService, FailService>();
        return services;
    }
}
