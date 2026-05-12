using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Support.Application.UseCases.CreateSupportContactMessage;

public interface ICreateSupportContactMessageService
{
    Task<Result<CreateSupportContactMessageResult>> CreateAsync(CreateSupportContactMessageInput input, CancellationToken ct);
}
