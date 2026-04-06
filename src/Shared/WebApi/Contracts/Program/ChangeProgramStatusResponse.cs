using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Program;

public sealed record ChangeProgramStatusResponse(
    Guid ProgramId,
    ProgramStatus Status,
    DateTime? CompletedAtUtc);

