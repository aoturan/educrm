namespace EduCrm.WebApi.Contracts.Support;

public sealed record CreateSupportRequestRequest(
    string Subject,
    string Message,
    string? PageUrl);

public sealed record CreateSupportRequestResponse(Guid SupportRequestId);

