namespace EduCrm.Modules.Support.Application.UseCases.MarkSupportContactMessageHandled;

public sealed record MarkSupportContactMessageHandledResult(
    Guid SupportContactMessageId,
    string Status,
    DateTime ReviewedAt);
