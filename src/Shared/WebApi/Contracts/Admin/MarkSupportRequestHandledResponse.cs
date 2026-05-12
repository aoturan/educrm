namespace EduCrm.WebApi.Contracts.Admin;

public sealed record MarkSupportRequestHandledResponse(
    Guid SupportRequestId,
    string Status,
    DateTime HandledAtUtc);
