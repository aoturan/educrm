using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Application.UseCases.GetMe;

public sealed record GetMeResult(
    string Email,
    string FullName,
    string Initials,
    UserRole Role);