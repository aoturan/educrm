using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Program;

public sealed record ChangeProgramStatusRequest(
    ProgramStatus Status);

