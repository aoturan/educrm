using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.CreateFollowUp;

public interface ICreateFollowUpService
{
    Task<Result<CreateFollowUpResult>> CreateAsync(CreateFollowUpInput input, CancellationToken ct);
}

