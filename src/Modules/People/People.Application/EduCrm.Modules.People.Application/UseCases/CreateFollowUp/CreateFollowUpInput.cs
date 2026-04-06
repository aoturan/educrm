using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.Modules.People.Application.UseCases.CreateFollowUp;

public sealed record CreateFollowUpInput(
    Guid PersonId,
    FollowUpType Type,
    string Title,
    DateTime DueAtUtc,
    Guid? ProgramId = null,
    string? Note = null);

