using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.SnoozeFollowUp;

public interface ISnoozeFollowUpService
{
    Task<Result<SnoozeFollowUpResult>> SnoozeAsync(SnoozeFollowUpInput input, CancellationToken ct);
}

