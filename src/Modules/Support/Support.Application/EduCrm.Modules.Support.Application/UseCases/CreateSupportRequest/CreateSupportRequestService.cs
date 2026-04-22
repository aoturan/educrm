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
    IOrgContext orgContext,
    ICurrentUser currentUser) : ICreateSupportRequestService
{
    public async Task<Result<CreateSupportRequestResult>> CreateAsync(CreateSupportRequestInput input, CancellationToken ct)
    {
        if (currentUser.UserId is null)
            return Result<CreateSupportRequestResult>.Fail(CommonErrors.Unauthorized());

        if (orgContext.OrganizationId is null)
            return Result<CreateSupportRequestResult>.Fail(CommonErrors.Forbidden("Organization scope is missing."));

        var supportRequest = new SupportRequest(
            orgContext.OrganizationId.Value,
            currentUser.UserId.Value,
            input.Subject,
            input.Message,
            clock.UtcNow.UtcDateTime,
            input.PageUrl);

        supportRequestRepo.Add(supportRequest);
        await uow.SaveChangesAsync(ct);

        return Result<CreateSupportRequestResult>.Success(new CreateSupportRequestResult(supportRequest.Id));
    }
}

