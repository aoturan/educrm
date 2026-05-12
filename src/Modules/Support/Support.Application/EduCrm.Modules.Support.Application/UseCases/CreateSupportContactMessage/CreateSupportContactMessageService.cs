using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Support.Application.Repositories;
using EduCrm.Modules.Support.Domain.Entities;
using EduCrm.SharedKernel.Abstractions;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Support.Application.UseCases.CreateSupportContactMessage;

public sealed class CreateSupportContactMessageService(
    ISupportContactMessageRepository supportContactMessageRepo,
    IUnitOfWork uow,
    IClock clock) : ICreateSupportContactMessageService
{
    public async Task<Result<CreateSupportContactMessageResult>> CreateAsync(CreateSupportContactMessageInput input, CancellationToken ct)
    {
        var contactMessage = new SupportContactMessage(
            input.FullName,
            input.Email,
            input.Subject,
            input.Message,
            clock.UtcNow.UtcDateTime);

        supportContactMessageRepo.Add(contactMessage);
        await uow.SaveChangesAsync(ct);

        return Result<CreateSupportContactMessageResult>.Success(new CreateSupportContactMessageResult(contactMessage.Id));
    }
}
