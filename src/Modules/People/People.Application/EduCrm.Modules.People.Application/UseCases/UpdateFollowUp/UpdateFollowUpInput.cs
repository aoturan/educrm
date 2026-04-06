using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.Modules.People.Application.UseCases.UpdateFollowUp;

public sealed record UpdateFollowUpInput(
    Guid FollowUpId,
    Guid PersonId,
    FollowUpType Type,
    string Title,
    DateTime DueAtUtc,
    Guid? ProgramId = null,
    string? Note = null);

