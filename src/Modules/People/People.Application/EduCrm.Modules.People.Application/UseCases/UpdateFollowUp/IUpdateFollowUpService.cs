using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.UpdateFollowUp;

public interface IUpdateFollowUpService
{
    Task<Result<UpdateFollowUpResult>> UpdateAsync(UpdateFollowUpInput input, CancellationToken ct);
}

