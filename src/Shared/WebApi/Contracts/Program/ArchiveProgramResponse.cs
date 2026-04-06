using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Program;

public sealed record ArchiveProgramResponse(Guid ProgramId, bool IsArchived, DateTime? ArchivedAtUtc, ProgramStatus Status);

