namespace EduCrm.Modules.Program.Application.UseCases.PublishProgram;

public sealed record PublishProgramResult(Guid ProgramId, string PublicSlug, DateTime PublicPublishedAtUtc);

