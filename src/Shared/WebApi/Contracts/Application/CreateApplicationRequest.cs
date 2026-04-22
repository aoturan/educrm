namespace EduCrm.WebApi.Contracts.Application;

public sealed record CreateApplicationRequest(
    string ProgramSlug,
    string? SubmittedFullName,
    string SubmittedPhone,
    string SubmittedEmail,
    string? SubmittedMessage);

