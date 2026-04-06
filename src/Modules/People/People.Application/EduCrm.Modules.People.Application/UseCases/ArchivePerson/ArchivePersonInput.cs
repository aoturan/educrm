namespace EduCrm.Modules.People.Application.UseCases.ArchivePerson;

public sealed record ArchivePersonInput(Guid PersonId, bool ShouldArchive);

