using EduCrm.Modules.Program.Domain.Enums;
using EduCrm.SharedKernel.Results;

namespace EduCrm.Modules.Program.Application.UseCases.GetApplicationById;

public sealed record ApplicationPersonInfo(Guid PersonId, string FullName);
public sealed record ApplicationProgramInfo(Guid ProgramId, string Name);

public sealed record GetApplicationByIdResult(
    Guid Id,
    ApplicationStatus Status,
    string SubmittedFullName,
    string SubmittedPhone,
    string? SubmittedMessage,
    DateTime FirstSubmittedAtUtc,
    DateTime LastSubmittedAtUtc,
    int SubmissionCount,
    DateTime? ConvertedAtUtc,
    DateTime? ClosedAtUtc,
    string? ClosedNote,
    ApplicationPersonInfo? Person,
    ApplicationProgramInfo? Program);

public interface IGetApplicationByIdService
{
    Task<Result<GetApplicationByIdResult>> GetAsync(Guid applicationId, CancellationToken ct);
}

