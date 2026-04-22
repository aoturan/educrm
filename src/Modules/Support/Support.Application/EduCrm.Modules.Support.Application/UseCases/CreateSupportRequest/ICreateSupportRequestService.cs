using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Support.Application.UseCases.CreateSupportRequest;

public interface ICreateSupportRequestService
{
    Task<Result<CreateSupportRequestResult>> CreateAsync(CreateSupportRequestInput input, CancellationToken ct);
}

