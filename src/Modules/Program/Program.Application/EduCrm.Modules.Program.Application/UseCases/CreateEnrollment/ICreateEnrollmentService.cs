using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.CreateEnrollment;

public interface ICreateEnrollmentService
{
    Task<Result<CreateEnrollmentResult>> CreateAsync(CreateEnrollmentInput input, CancellationToken ct);
}

