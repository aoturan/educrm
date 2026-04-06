using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Application.UseCases.ChangeStatus;

public sealed record ChangeProgramStatusResult(
    Guid ProgramId,
    ProgramStatus Status,
    DateTime? CompletedAtUtc);

