namespace EduCrm.WebApi.Contracts.Program;

public sealed record PublishProgramResponse(Guid ProgramId, string PublicSlug, DateTime PublicPublishedAtUtc);

