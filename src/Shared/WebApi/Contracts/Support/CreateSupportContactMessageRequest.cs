namespace EduCrm.WebApi.Contracts.Support;

public sealed record CreateSupportContactMessageRequest(
    string FullName,
    string Email,
    string Subject,
    string Message);

public sealed record CreateSupportContactMessageResponse(Guid SupportContactMessageId);
