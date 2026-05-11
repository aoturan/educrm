namespace EduCrm.Modules.Account.Application.SubscriptionRequests;

public interface IPaymentReferenceCodeGenerator
{
    Task<string> GenerateAsync(CancellationToken ct);
}