namespace EduCrm.Modules.Program.Application.UseCases.CreateApplication;

public sealed record CreateApplicationInput(
    string ProgramSlug,
    string? SubmittedFullName,
    string SubmittedPhone,
    string SubmittedEmail,
    string? SubmittedMessage);

