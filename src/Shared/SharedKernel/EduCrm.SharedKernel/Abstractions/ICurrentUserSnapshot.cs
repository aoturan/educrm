using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.SharedKernel.Abstractions;

public interface ICurrentUserSnapshot
{
    bool IsLoaded { get; }
    Guid UserId { get; }
    Guid OrganizationId { get; }
    string OrganizationName { get; }
    string Email { get; }
    string FullName { get; }
    bool IsActive { get; }
    bool IsApplicationAdmin { get; }
    UserRole Role { get; }
}
