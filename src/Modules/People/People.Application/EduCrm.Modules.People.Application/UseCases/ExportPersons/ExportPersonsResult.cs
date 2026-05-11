using EduCrm.Modules.People.Application.Repositories;

namespace EduCrm.Modules.People.Application.UseCases.ExportPersons;

public sealed record ExportPersonsResult(
    IReadOnlyList<PersonExportItemData> Rows,
    DateTime GeneratedAtUtc);
