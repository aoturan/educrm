using EduCrm.Modules.People.Application.Repositories;

namespace EduCrm.Modules.People.Application.UseCases.ListPersons;

public sealed record ListPersonsResult(
    IReadOnlyList<PersonListItemData> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int EnrolledCount,
    int NotEnrolledCount);
