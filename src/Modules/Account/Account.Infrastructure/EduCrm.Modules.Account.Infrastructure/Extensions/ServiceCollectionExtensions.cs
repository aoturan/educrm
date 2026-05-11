using Amazon.S3;
using EduCrm.Modules.Account.Application.Email;
using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.Security;
using EduCrm.Modules.Account.Application.SubscriptionRequests;
using EduCrm.Modules.Account.Contracts.Abstractions;
using EduCrm.Modules.Account.Infrastructure.Email;
using EduCrm.Modules.Account.Infrastructure.Queries;
using EduCrm.Modules.Account.Infrastructure.Repositories;
using EduCrm.Modules.Account.Infrastructure.Security;
using EduCrm.Modules.Account.Infrastructure.Storage;
using EduCrm.Modules.Account.Infrastructure.SubscriptionRequests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EduCrm.Modules.Account.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountInfra(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<ISubscriptionRequestRepository, SubscriptionRequestRepository>();
        services.AddScoped<ISubscriptionHistoryRepository, SubscriptionHistoryRepository>();
        services.AddScoped<ISubscriptionPaymentNotificationRepository, SubscriptionPaymentNotificationRepository>();
        services.AddScoped<IOrganizationBillingDetailRepository, OrganizationBillingDetailRepository>();
        services.AddScoped<IUserOrganizationResolver, UserOrganizationResolver>();
        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPlanLimitsResolver, PlanLimitsResolver>();
        services.AddScoped<IPaymentReferenceCodeGenerator, PaymentReferenceCodeGenerator>();
        services.AddScoped<IPlanPricingResolver, PlanPricingResolver>();
        services.AddSingleton<IBankAccountResolver, BankAccountResolver>();
        services.AddScoped<IPlanUsageResolver, PlanUsageResolver>();
        services.AddScoped<IExportRateLimiter, ExportRateLimiter>();

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<DigitalOceanSpacesOptions>>().Value;
            var config = new AmazonS3Config
            {
                ServiceURL = opts.ServiceUrl,
                AuthenticationRegion = opts.Region,
                ForcePathStyle = false
            };
            return new AmazonS3Client(opts.AccessKeyId, opts.SecretKey, config);
        });
        services.AddScoped<IReceiptStorage, DigitalOceanSpacesReceiptStorage>();

        services.AddHttpClient<IEmailSender, ResendEmailSender>(http =>
        {
            http.BaseAddress = new Uri("https://api.resend.com/");
        });

        return services;
    }
}