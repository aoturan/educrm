using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Application.UseCases.ArchiveProgram;

public sealed record ArchiveProgramResult(Guid ProgramId, bool IsArchived, DateTime? ArchivedAtUtc, ProgramStatus Status);

