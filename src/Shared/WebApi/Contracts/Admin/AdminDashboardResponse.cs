namespace EduCrm.WebApi.Contracts.Admin;

public sealed record AdminDashboardResponse(
    AdminDashboardCountsResponse Counts,
    IReadOnlyList<AdminPendingPaymentRequestResponse> PendingPaymentRequests);

public sealed record AdminDashboardCountsResponse(
    int PendingPaymentRequests,
    int NewOrganizationsThisWeek,
    int ActivePaidOrganizations,
    int FreeOrganizations);

public sealed record AdminPendingPaymentRequestResponse(
    Guid SubscriptionRequestId,
    Guid OrganizationId,
    string OrganizationName,
    string RequestedPlanCode,
    string Status,
    decimal Amount,
    string PaymentReferenceCode,
    DateTime RequestedAtUtc,
    bool HasPaymentNotification);
