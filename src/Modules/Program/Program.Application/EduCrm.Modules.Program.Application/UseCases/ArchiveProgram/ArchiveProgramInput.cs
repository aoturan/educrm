namespace EduCrm.Modules.Program.Application.UseCases.ArchiveProgram;

public sealed record ArchiveProgramInput(Guid ProgramId, bool ShouldArchive);

