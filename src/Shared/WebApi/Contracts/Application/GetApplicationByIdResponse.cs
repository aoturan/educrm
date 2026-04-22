using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Application;

public sealed record ApplicationPersonInfoResponse(Guid PersonId, string FullName);
public sealed record ApplicationProgramInfoResponse(Guid ProgramId, string Name);

public sealed record GetApplicationByIdResponse(
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
    ApplicationPersonInfoResponse? Person,
    ApplicationProgramInfoResponse? Program);

