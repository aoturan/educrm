namespace EduCrm.Modules.Program.Contracts.Abstractions;

public record ProgramSummary(Guid Id, Guid OrganizationId, string Name);

public interface IProgramReader
{
    Task<ProgramSummary?> GetProgramByIdAsync(Guid programId, Guid organizationId, CancellationToken ct);
}