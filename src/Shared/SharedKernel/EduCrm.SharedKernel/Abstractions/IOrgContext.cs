namespace EduCrm.SharedKernel.Abstractions;

public interface IOrgContext
{
    bool HasOrganization { get; }
    Guid? OrganizationId { get; }
}