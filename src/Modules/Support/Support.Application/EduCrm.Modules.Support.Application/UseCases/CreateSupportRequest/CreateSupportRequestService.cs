using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Support.Application.Repositories;
using EduCrm.Modules.Support.Domain.Entities;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Errors;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Support.Application.UseCases.CreateSupportRequest;

public sealed class CreateSupportRequestService(
    ISupportRequestRepository supportRequestRepo,
    IUnitOfWork uow,
    IClock clock,
    ICurrentUserSnapshot user) : ICreateSupportRequestService
{
    public async Task<Result<CreateSupportRequestResult>> CreateAsync(CreateSupportRequestInput input, CancellationToken ct)
    {
        var supportRequest = new SupportRequest(
            user.OrganizationId,
            user.UserId,
            input.Subject,
            input.Message,
            clock.UtcNow.UtcDateTime,
            input.PageUrl);

        supportRequestRepo.Add(supportRequest);
        await uow.SaveChangesAsync(ct);

        return Result<CreateSupportRequestResult>.Success(new CreateSupportRequestResult(supportRequest.Id));
    }
}

