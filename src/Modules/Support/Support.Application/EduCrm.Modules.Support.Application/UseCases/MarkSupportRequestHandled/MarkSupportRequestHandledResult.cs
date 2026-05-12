namespace EduCrm.Modules.Support.Application.UseCases.MarkSupportRequestHandled;

public sealed record MarkSupportRequestHandledResult(
    Guid SupportRequestId,
    string Status,
    DateTime HandledAtUtc);
