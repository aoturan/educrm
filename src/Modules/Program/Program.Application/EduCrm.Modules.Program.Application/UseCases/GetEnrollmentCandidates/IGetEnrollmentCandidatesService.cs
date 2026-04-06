using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.GetEnrollmentCandidates;

public interface IGetEnrollmentCandidatesService
{
    Task<Result<GetEnrollmentCandidatesResult>> GetAsync(GetEnrollmentCandidatesInput input, CancellationToken ct);
}

