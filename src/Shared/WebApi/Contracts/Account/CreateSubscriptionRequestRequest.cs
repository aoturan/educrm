using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.WebApi.Contracts.Account;

public sealed record CreateSubscriptionRequestRequest(PlanCode RequestedPlanCode);