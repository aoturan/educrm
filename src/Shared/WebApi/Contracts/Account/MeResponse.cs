using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Account;

public sealed record MeResponse(
    string Email,
    string FullName,
    string Initials,
    UserRole Role);