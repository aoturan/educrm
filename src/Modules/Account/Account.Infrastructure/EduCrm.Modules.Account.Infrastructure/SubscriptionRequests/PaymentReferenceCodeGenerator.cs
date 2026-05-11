using EduCrm.Modules.Account.Application.Repositories;
using EduCrm.Modules.Account.Application.SubscriptionRequests;

namespace EduCrm.Modules.Account.Infrastructure.SubscriptionRequests;

public sealed class PaymentReferenceCodeGenerator(ISubscriptionRequestRepository requestRepo)
    : IPaymentReferenceCodeGenerator
{
    private const int Attempts = 10;
    private const int Min = 100000;
    private const int MaxExclusive = 1000000;

    public async Task<string> GenerateAsync(CancellationToken ct)
    {
        for (var i = 0; i < Attempts; i++)
        {
            var candidate = Random.Shared.Next(Min, MaxExclusive).ToString();
            if (!await requestRepo.ExistsByReferenceCodeAsync(candidate, ct))
                return candidate;
        }

        throw new InvalidOperationException("Unable to generate a unique payment reference code.");
    }
}