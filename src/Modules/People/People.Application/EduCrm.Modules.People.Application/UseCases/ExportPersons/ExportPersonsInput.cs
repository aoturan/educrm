namespace EduCrm.Modules.People.Application.UseCases.ExportPersons;

public sealed record ExportPersonsInput(
    string? SearchTerm = null,
    string? PreFilter = null,
    bool ShowArchived = false);
