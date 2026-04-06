namespace EduCrm.WebApi.Contracts.Person;

public sealed record PersonListItemResponse(
    Guid PersonId,
    string FullName,
    string? Email,
    string? Phone,
    int EnrolledProgramCount,
    bool HasActiveEnrollment,
    bool IsArchived);

public sealed record PersonListResponse(
    IReadOnlyList<PersonListItemResponse> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int EnrolledCount,
    int NotEnrolledCount);

