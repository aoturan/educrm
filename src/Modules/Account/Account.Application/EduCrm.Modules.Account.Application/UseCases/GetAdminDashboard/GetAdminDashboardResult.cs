namespace EduCrm.Modules.Account.Application.UseCases.GetAdminDashboard;

public sealed record GetAdminDashboardResult(
    AdminDashboardCountsResult Counts,
    IReadOnlyList<AdminPendingPaymentRequestItem> PendingPaymentRequests);

public sealed record AdminDashboardCountsResult(
    int PendingPaymentRequests,
    int NewOrganizationsThisWeek,
    int ActivePaidOrganizations,
    int FreeOrganizations,
    int NewSupportContactMessages,
    int NewSupportRequests);

public sealed record AdminPendingPaymentRequestItem(
    Guid SubscriptionRequestId,
    Guid OrganizationId,
    string OrganizationName,
    string RequestedPlanCode,
    string Status,
    decimal Amount,
    string PaymentReferenceCode,
    DateTime RequestedAtUtc,
    bool HasPaymentNotification);
