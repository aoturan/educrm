namespace EduCrm.WebApi.Contracts.Person;

public sealed record ArchivePersonResponse(Guid PersonId, bool IsArchived, DateTime? ArchivedAtUtc);

