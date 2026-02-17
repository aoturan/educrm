using EduCrm.SharedKernel.Abstractions;

namespace EduCrm.Infrastructure.Tenancy;

public sealed class OrgContext : IOrgContext, IOrgContextWriter
{
    public bool HasOrganization => OrganizationId.HasValue;
    public Guid? OrganizationId { get; private set; }

    // Only middleware/host should set this via writer interface
    public void Set(Guid organizationId)
    {
        OrganizationId = organizationId;
    }
}

// Writer contract for setting org id (kept in Infrastructure)
public interface IOrgContextWriter
{
    void Set(Guid organizationId);
}