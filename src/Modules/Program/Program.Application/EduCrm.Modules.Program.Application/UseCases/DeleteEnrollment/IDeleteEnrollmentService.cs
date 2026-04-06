using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.DeleteEnrollment;

public interface IDeleteEnrollmentService
{
    Task<Result> DeleteAsync(Guid enrollmentId, CancellationToken ct);
}

