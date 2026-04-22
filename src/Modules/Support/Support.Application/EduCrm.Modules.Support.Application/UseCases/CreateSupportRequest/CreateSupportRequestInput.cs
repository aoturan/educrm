namespace EduCrm.Modules.Support.Application.UseCases.CreateSupportRequest;

public sealed record CreateSupportRequestInput(
    string Subject,
    string Message,
    string? PageUrl);

