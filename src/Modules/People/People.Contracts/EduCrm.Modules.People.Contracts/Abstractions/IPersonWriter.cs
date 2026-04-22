namespace EduCrm.Modules.People.Contracts.Abstractions;

public interface IPersonWriter
{
    Guid AddFromApplication(
        Guid organizationId,
        string fullName,
        string? phone,
        string? email,
        DateTime nowUtc);
}

