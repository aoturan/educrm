namespace EduCrm.Modules.Program.Application.UseCases.CreateEnrollment;

public sealed record CreateEnrollmentInput(
    Guid ProgramId,
    Guid PersonId);

