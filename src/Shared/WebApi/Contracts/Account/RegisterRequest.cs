using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Account;

public sealed record RegisterRequest(
    string Name,
    string Email,
    string OrganizationName,
    string Password,
    string Phone,
    PlanCode PlanCode,
    string TurnstileToken);
