using EduCrm.Modules.Account.Domain.Enums;
using EduCrm.SharedKernel.Abstractions;

namespace EduCrm.Infrastructure.Auth;

public sealed class CurrentUserSnapshot : ICurrentUserSnapshot
{
    private Guid _userId;
    private Guid _orgId;
    private string? _organizationName;
    private string? _email;
    private string? _fullName;

    public bool IsLoaded { get; private set; }
    public Guid UserId => IsLoaded ? _userId : throw new InvalidOperationException("CurrentUserSnapshot is not loaded.");
    public Guid OrganizationId => IsLoaded ? _orgId : throw new InvalidOperationException("CurrentUserSnapshot is not loaded.");
    public string OrganizationName => IsLoaded ? _organizationName! : throw new InvalidOperationException("CurrentUserSnapshot is not loaded.");
    public string Email => IsLoaded ? _email! : throw new InvalidOperationException("CurrentUserSnapshot is not loaded.");
    public string FullName => IsLoaded ? _fullName! : throw new InvalidOperationException("CurrentUserSnapshot is not loaded.");
    public bool IsActive { get; private set; }
    public bool IsApplicationAdmin { get; private set; }
    public UserRole Role { get; private set; }

    public void Set(Guid userId, Guid organizationId, string organizationName, string email, string fullName, bool isActive, bool isApplicationAdmin, UserRole role)
    {
        _userId = userId;
        _orgId = organizationId;
        _organizationName = organizationName;
        _email = email;
        _fullName = fullName;
        IsActive = isActive;
        IsApplicationAdmin = isApplicationAdmin;
        Role = role;
        IsLoaded = true;
    }
}
