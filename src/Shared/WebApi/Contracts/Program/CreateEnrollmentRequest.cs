namespace EduCrm.WebApi.Contracts.Program;

public sealed record CreateEnrollmentRequest(
    Guid ProgramId,
    Guid PersonId);

