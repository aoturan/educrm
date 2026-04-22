namespace EduCrm.Modules.People.Contracts.Abstractions;

public record PersonContactMatch(
    Guid PersonId,
    string FullName,
    string? Email,
    string? Phone);

public interface IPersonReader
{
    Task<IReadOnlyList<PersonContactMatch>> FindByContactAsync(
        string email,
        string phone,
        Guid organizationId,
        CancellationToken ct);

    Task<PersonContactMatch?> GetByIdAsync(Guid personId, Guid organizationId, CancellationToken ct);

    Task<bool> ExistsInOrganizationAsync(Guid personId, Guid organizationId, CancellationToken ct);
}
