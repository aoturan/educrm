using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.People.Application.UseCases.RescheduleDueDate;

public interface IRescheduleDueDateService
{
    Task<Result<RescheduleDueDateResult>> RescheduleAsync(RescheduleDueDateInput input, CancellationToken ct);
}

