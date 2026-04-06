using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Application.UseCases.ChangeStatus;

public sealed record ChangeProgramStatusInput(
    Guid ProgramId,
    ProgramStatus Status);

