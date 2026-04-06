namespace EduCrm.Modules.People.Application.UseCases.ArchivePerson;

public sealed record ArchivePersonResult(Guid PersonId, bool IsArchived, DateTime? ArchivedAtUtc);

