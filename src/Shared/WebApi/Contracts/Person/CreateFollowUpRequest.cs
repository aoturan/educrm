using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Person;

public sealed record CreateFollowUpRequest(
    Guid PersonId,
    FollowUpType Type,
    string Title,
    DateTime DueAtUtc,
    Guid? ProgramId = null,
    string? Note = null);

