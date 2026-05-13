using EduCrm.Modules.Account.Application.UseCases.ApproveSubscriptionRequest;
using EduCrm.Modules.Account.Application.UseCases.DownloadPaymentNotificationReceipt;
using EduCrm.Modules.Account.Application.UseCases.GetAdminDashboard;
using EduCrm.Modules.Account.Application.UseCases.GetOrganizationOverview;
using EduCrm.Modules.Account.Application.UseCases.GetSubscriptionRequestDetail;
using EduCrm.Modules.Account.Application.UseCases.MarkSubscriptionRequestInvoiced;
using EduCrm.Modules.Account.Application.UseCases.OverrideOrganizationSubscription;
using EduCrm.Modules.Account.Application.UseCases.ListOrganizations;
using EduCrm.Modules.Account.Application.UseCases.ListSubscriptionRequests;
using EduCrm.Modules.Support.Application.UseCases.ListSupportContactMessages;
using EduCrm.Modules.Support.Application.UseCases.ListSupportRequests;
using EduCrm.Modules.Support.Application.UseCases.MarkSupportContactMessageHandled;
using EduCrm.Modules.Support.Application.UseCases.MarkSupportRequestHandled;
using EduCrm.WebApi.Contracts.Admin;
using EduCrm.WebApi.Extensions;
using EduCrm.WebApi.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCrm.WebApi.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = "ApplicationAdmin")]
[Authorize(Policy = "ActiveUser")]
public sealed class AdminController : ControllerBase
{
    private readonly IGetAdminDashboardService _getDashboard;
    private readonly IListOrganizationsService _listOrganizations;
    private readonly IGetOrganizationOverviewService _getOrganizationOverview;
    private readonly IGetSubscriptionRequestDetailService _getSubscriptionRequestDetail;
    private readonly IDownloadPaymentNotificationReceiptService _downloadReceipt;
    private readonly IApproveSubscriptionRequestService _approveSubscriptionRequest;
    private readonly IMarkSubscriptionRequestInvoicedService _markInvoiced;
    private readonly IOverrideOrganizationSubscriptionService _overrideSubscription;
    private readonly IListSubscriptionRequestsService _listSubscriptionRequests;
    private readonly IListSupportContactMessagesService _listSupportContactMessages;
    private readonly IMarkSupportContactMessageHandledService _markSupportContactMessageHandled;
    private readonly IListSupportRequestsService _listSupportRequests;
    private readonly IMarkSupportRequestHandledService _markSupportRequestHandled;
    private readonly IRequestValidator _validator;

    public AdminController(
        IGetAdminDashboardService getDashboard,
        IListOrganizationsService listOrganizations,
        IGetOrganizationOverviewService getOrganizationOverview,
        IGetSubscriptionRequestDetailService getSubscriptionRequestDetail,
        IDownloadPaymentNotificationReceiptService downloadReceipt,
        IApproveSubscriptionRequestService approveSubscriptionRequest,
        IMarkSubscriptionRequestInvoicedService markInvoiced,
        IOverrideOrganizationSubscriptionService overrideSubscription,
        IListSubscriptionRequestsService listSubscriptionRequests,
        IListSupportContactMessagesService listSupportContactMessages,
        IMarkSupportContactMessageHandledService markSupportContactMessageHandled,
        IListSupportRequestsService listSupportRequests,
        IMarkSupportRequestHandledService markSupportRequestHandled,
        IRequestValidator validator)
    {
        _getDashboard = getDashboard;
        _listOrganizations = listOrganizations;
        _getOrganizationOverview = getOrganizationOverview;
        _getSubscriptionRequestDetail = getSubscriptionRequestDetail;
        _downloadReceipt = downloadReceipt;
        _approveSubscriptionRequest = approveSubscriptionRequest;
        _markInvoiced = markInvoiced;
        _overrideSubscription = overrideSubscription;
        _listSubscriptionRequests = listSubscriptionRequests;
        _listSupportContactMessages = listSupportContactMessages;
        _markSupportContactMessageHandled = markSupportContactMessageHandled;
        _listSupportRequests = listSupportRequests;
        _markSupportRequestHandled = markSupportRequestHandled;
        _validator = validator;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard(CancellationToken ct)
    {
        var result = await _getDashboard.GetAsync(ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new AdminDashboardResponse(
                new AdminDashboardCountsResponse(
                    r.Counts.PendingPaymentRequests,
                    r.Counts.NewOrganizationsThisWeek,
                    r.Counts.ActivePaidOrganizations,
                    r.Counts.FreeOrganizations,
                    r.Counts.NewSupportContactMessages,
                    r.Counts.NewSupportRequests),
                r.PendingPaymentRequests
                    .Select(p => new AdminPendingPaymentRequestResponse(
                        p.SubscriptionRequestId,
                        p.OrganizationId,
                        p.OrganizationName,
                        p.RequestedPlanCode,
                        p.Status,
                        p.Amount,
                        p.PaymentReferenceCode,
                        p.RequestedAtUtc,
                        p.HasPaymentNotification))
                    .ToList())));
    }

    [HttpGet("organizations")]
    public async Task<IActionResult> ListOrganizations(
        [FromQuery] ListOrganizationsQuery query,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(query, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize is < 1 or > 100 ? 10 : query.PageSize;

        var input = new ListOrganizationsInput(page, pageSize, query.Q, query.Face);
        var result = await _listOrganizations.ListAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new OrganizationListPagedResponse(
                r.Items.Select(x => new OrganizationListItemResponse(
                    x.Id,
                    x.Name,
                    x.ContactName,
                    x.ContactEmail,
                    x.ContactPhone,
                    x.CreatedAtUtc)).ToList(),
                r.Page,
                r.PageSize,
                r.TotalCount)));
    }

    [HttpPost("organizations/{organizationId:guid}/subscription")]
    public async Task<IActionResult> OverrideOrganizationSubscription(
        Guid organizationId,
        [FromBody] OverrideOrganizationSubscriptionRequest req,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(req, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var input = new OverrideOrganizationSubscriptionInput(
            organizationId, req.PlanCode, req.StartsAtUtc, req.EndsAtUtc);

        var result = await _overrideSubscription.OverrideAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new OverrideOrganizationSubscriptionResponse(
                r.SubscriptionId,
                r.OrganizationId,
                r.PlanCode,
                r.StartsAtUtc,
                r.EndsAtUtc,
                r.UpdatedAtUtc)));
    }

    [HttpGet("organizations/{organizationId:guid}")]
    public async Task<IActionResult> GetOrganizationOverview(Guid organizationId, CancellationToken ct)
    {
        var result = await _getOrganizationOverview.GetAsync(new GetOrganizationOverviewInput(organizationId), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new OrganizationOverviewResponse(
                r.OrganizationId,
                r.OrganizationName,
                r.OwnerFullName,
                r.OwnerEmail,
                r.ContactPhone,
                r.CreatedAtUtc,
                new OrganizationOverviewLimitsResponse(
                    r.Limits.PlanCode,
                    r.Limits.Status,
                    r.Limits.ExportEnabled,
                    new LimitUsageResponse(r.Limits.Users.Limit, r.Limits.Users.Current),
                    new LimitUsageResponse(r.Limits.ActivePersons.Limit, r.Limits.ActivePersons.Current),
                    new LimitUsageResponse(r.Limits.ActivePrograms.Limit, r.Limits.ActivePrograms.Current),
                    new LimitUsageResponse(r.Limits.OpenFollowUps.Limit, r.Limits.OpenFollowUps.Current)),
                r.Subscription is null
                    ? null
                    : new OrganizationOverviewSubscriptionResponse(
                        r.Subscription.Id,
                        r.Subscription.PlanCode,
                        r.Subscription.StartsAtUtc,
                        r.Subscription.EndsAtUtc,
                        r.Subscription.DowngradedFromPlanCode,
                        r.Subscription.DowngradedAtUtc,
                        r.Subscription.CreatedAtUtc,
                        r.Subscription.UpdatedAtUtc),
                r.BillingDetail is null
                    ? null
                    : new OrganizationOverviewBillingDetailResponse(
                        r.BillingDetail.Id,
                        r.BillingDetail.BillingType,
                        r.BillingDetail.BillingName,
                        r.BillingDetail.TaxNumber,
                        r.BillingDetail.TaxOffice,
                        r.BillingDetail.BillingEmail,
                        r.BillingDetail.BillingAddress,
                        r.BillingDetail.CreatedAtUtc,
                        r.BillingDetail.UpdatedAtUtc),
                r.SubscriptionRequests
                    .Select(sr => new OrganizationOverviewSubscriptionRequestResponse(
                        sr.Id,
                        sr.RequestedPlanCode,
                        sr.Status,
                        sr.PaymentMethod,
                        sr.Amount,
                        sr.PaymentReferenceCode,
                        sr.RequestedAtUtc,
                        sr.ExpiresAtUtc,
                        sr.ApprovedAtUtc,
                        sr.RejectedAtUtc,
                        sr.CancelledAtUtc,
                        sr.IsInvoiced,
                        sr.HasPaymentNotification))
                    .ToList())));
    }

    [HttpGet("subscription-requests")]
    public async Task<IActionResult> ListSubscriptionRequests(
        [FromQuery] ListSubscriptionRequestsQuery query,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(query, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize is < 1 or > 100 ? 10 : query.PageSize;

        var input = new ListSubscriptionRequestsInput(page, pageSize, query.Q, query.Face);
        var result = await _listSubscriptionRequests.ListAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new SubscriptionRequestListPagedResponse(
                r.Items.Select(x => new SubscriptionRequestListItemResponse(
                    x.Id,
                    x.OrganizationId,
                    x.OrganizationName,
                    x.RequestedPlanCode,
                    x.Status,
                    x.PaymentMethod,
                    x.Amount,
                    x.PaymentReferenceCode,
                    x.RequestedAtUtc,
                    x.ExpiresAtUtc,
                    x.ApprovedAtUtc,
                    x.RejectedAtUtc,
                    x.CancelledAtUtc,
                    x.IsInvoiced,
                    x.HasPaymentNotification)).ToList(),
                r.Page,
                r.PageSize,
                r.TotalCount)));
    }

    [HttpPost("subscription-requests/{subscriptionRequestId:guid}/mark-invoiced")]
    public async Task<IActionResult> MarkSubscriptionRequestInvoiced(Guid subscriptionRequestId, CancellationToken ct)
    {
        var result = await _markInvoiced.MarkAsync(
            new MarkSubscriptionRequestInvoicedInput(subscriptionRequestId), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new MarkSubscriptionRequestInvoicedResponse(
                r.SubscriptionRequestId,
                r.IsInvoiced,
                r.UpdatedAtUtc)));
    }

    [HttpPost("subscription-requests/{subscriptionRequestId:guid}/approve")]
    public async Task<IActionResult> ApproveSubscriptionRequest(Guid subscriptionRequestId, CancellationToken ct)
    {
        var result = await _approveSubscriptionRequest.ApproveAsync(
            new ApproveSubscriptionRequestInput(subscriptionRequestId), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new ApproveSubscriptionRequestResponse(
                r.SubscriptionRequestId,
                r.ApprovedAtUtc,
                r.SubscriptionId,
                r.PlanCode,
                r.StartsAtUtc,
                r.EndsAtUtc)));
    }

    [HttpGet("subscription-requests/{subscriptionRequestId:guid}/receipt")]
    public async Task<IActionResult> DownloadSubscriptionRequestReceipt(Guid subscriptionRequestId, CancellationToken ct)
    {
        var result = await _downloadReceipt.DownloadAsync(
            new DownloadPaymentNotificationReceiptInput(subscriptionRequestId), ct);

        return result.ToActionResult(HttpContext, this, r =>
            File(r.Content, r.ContentType, r.FileName));
    }

    [HttpGet("subscription-requests/{subscriptionRequestId:guid}")]
    public async Task<IActionResult> GetSubscriptionRequestDetail(Guid subscriptionRequestId, CancellationToken ct)
    {
        var result = await _getSubscriptionRequestDetail.GetAsync(
            new GetSubscriptionRequestDetailInput(subscriptionRequestId), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new SubscriptionRequestDetailResponse(
                new SubscriptionRequestResponse(
                    r.Request.Id,
                    r.Request.OrganizationId,
                    r.Request.RequestedPlanCode,
                    r.Request.Status,
                    r.Request.PaymentMethod,
                    r.Request.Amount,
                    r.Request.PaymentReferenceCode,
                    r.Request.RequestedAtUtc,
                    r.Request.ExpiresAtUtc,
                    r.Request.ApprovedAtUtc,
                    r.Request.RejectedAtUtc,
                    r.Request.CancelledAtUtc,
                    r.Request.IsInvoiced,
                    r.Request.CreatedAtUtc,
                    r.Request.UpdatedAtUtc),
                r.PaymentNotification is null
                    ? null
                    : new PaymentNotificationResponse(
                        r.PaymentNotification.Id,
                        r.PaymentNotification.SubscriptionRequestId,
                        r.PaymentNotification.OrganizationId,
                        r.PaymentNotification.SenderName,
                        r.PaymentNotification.PaymentDate,
                        r.PaymentNotification.Amount,
                        r.PaymentNotification.Note,
                        r.PaymentNotification.ReceiptObjectKey,
                        r.PaymentNotification.ReceiptFileName,
                        r.PaymentNotification.ReceiptContentType,
                        r.PaymentNotification.ReceiptSizeBytes,
                        r.PaymentNotification.CreatedAtUtc,
                        r.PaymentNotification.UpdatedAtUtc))));
    }

    [HttpGet("support-contact-messages")]
    public async Task<IActionResult> ListSupportContactMessages(
        [FromQuery] ListSupportContactMessagesQuery query,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(query, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize is < 1 or > 100 ? 10 : query.PageSize;

        var input = new ListSupportContactMessagesInput(page, pageSize, query.PreFilter);
        var result = await _listSupportContactMessages.ListAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new SupportContactMessageListPagedResponse(
                r.Items.Select(x => new SupportContactMessageListItemResponse(
                    x.Id,
                    x.FullName,
                    x.Email,
                    x.Subject,
                    x.Message,
                    x.Status,
                    x.CreatedAt,
                    x.ReviewedAt)).ToList(),
                r.Page,
                r.PageSize,
                r.TotalCount)));
    }

    [HttpPost("support-contact-messages/{supportContactMessageId:guid}/handle")]
    public async Task<IActionResult> MarkSupportContactMessageHandled(
        Guid supportContactMessageId,
        CancellationToken ct)
    {
        var result = await _markSupportContactMessageHandled.MarkAsync(
            new MarkSupportContactMessageHandledInput(supportContactMessageId), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new MarkSupportContactMessageHandledResponse(
                r.SupportContactMessageId,
                r.Status,
                r.ReviewedAt)));
    }

    [HttpGet("support-requests")]
    public async Task<IActionResult> ListSupportRequests(
        [FromQuery] ListSupportRequestsQuery query,
        CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(query, ct);
        if (!validation.IsValid) return validation.ToValidationProblem(this);

        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize is < 1 or > 100 ? 10 : query.PageSize;

        var input = new ListSupportRequestsInput(page, pageSize, query.PreFilter);
        var result = await _listSupportRequests.ListAsync(input, ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new SupportRequestListPagedResponse(
                r.Items.Select(x => new SupportRequestListItemResponse(
                    x.Id,
                    x.OrganizationId,
                    x.OrganizationName,
                    x.PageUrl,
                    x.Subject,
                    x.Message,
                    x.Status,
                    x.CreatedAtUtc)).ToList(),
                r.Page,
                r.PageSize,
                r.TotalCount)));
    }

    [HttpPost("support-requests/{supportRequestId:guid}/handle")]
    public async Task<IActionResult> MarkSupportRequestHandled(
        Guid supportRequestId,
        CancellationToken ct)
    {
        var result = await _markSupportRequestHandled.MarkAsync(
            new MarkSupportRequestHandledInput(supportRequestId), ct);

        return result.ToActionResult(HttpContext, this, r =>
            Ok(new MarkSupportRequestHandledResponse(
                r.SupportRequestId,
                r.Status,
                r.HandledAtUtc)));
    }
}
