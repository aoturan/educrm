namespace EduCrm.Modules.Support.Application.UseCases.CreateSupportContactMessage;

public sealed record CreateSupportContactMessageInput(
    string FullName,
    string Email,
    string Subject,
    string Message);
