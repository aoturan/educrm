namespace EduCrm.WebApi.Contracts.Admin;

public sealed record MarkSupportContactMessageHandledResponse(
    Guid SupportContactMessageId,
    string Status,
    DateTime ReviewedAt);
