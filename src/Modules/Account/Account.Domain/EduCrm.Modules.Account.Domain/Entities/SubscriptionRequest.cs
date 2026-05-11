using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Domain.Entities;

[Table("accounts_subscription_requests")]
public sealed class SubscriptionRequest
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }

    [Required]
    [Column("organization_id")]
    public Guid OrganizationId { get; private set; }

    [Required]
    [Column("requested_plan_code")]
    public PlanCode RequestedPlanCode { get; private set; }

    [Required]
    [Column("status")]
    public RequestStatus Status { get; private set; }

    [Required]
    [Column("payment_method")]
    public PaymentMethod PaymentMethod { get; private set; }

    [Required]
    [Column("amount")]
    public decimal Amount { get; private set; }

    [Required]
    [Column("payment_reference_code")]
    [StringLength(50)]
    public string PaymentReferenceCode { get; private set; } = string.Empty;

    [Required]
    [Column("requested_at_utc")]
    public DateTime RequestedAtUtc { get; private set; }

    [Required]
    [Column("expires_at_utc")]
    public DateTime ExpiresAtUtc { get; private set; }

    [Column("approved_at_utc")]
    public DateTime? ApprovedAtUtc { get; private set; }

    [Column("rejected_at_utc")]
    public DateTime? RejectedAtUtc { get; private set; }

    [Column("cancelled_at_utc")]
    public DateTime? CancelledAtUtc { get; private set; }

    [Required]
    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; private set; }

    [Required]
    [Column("updated_at_utc")]
    public DateTime UpdatedAtUtc { get; private set; }

    [Required]
    [Column("is_invoiced")]
    public bool IsInvoiced { get; private set; }

    private SubscriptionRequest() { } // EF

    public SubscriptionRequest(
        Guid id,
        Guid organizationId,
        PlanCode requestedPlanCode,
        RequestStatus status,
        PaymentMethod paymentMethod,
        decimal amount,
        string paymentReferenceCode,
        DateTime requestedAtUtc,
        DateTime expiresAtUtc,
        DateTime createdAtUtc)
    {
        if (id == Guid.Empty) throw new ArgumentException("SubscriptionRequest id is required.", nameof(id));
        if (organizationId == Guid.Empty) throw new ArgumentException("Organization is required.", nameof(organizationId));

        Id = id;
        OrganizationId = organizationId;
        RequestedPlanCode = requestedPlanCode;
        Status = status;
        PaymentMethod = paymentMethod;
        Amount = amount;
        PaymentReferenceCode = paymentReferenceCode ?? string.Empty;
        RequestedAtUtc = requestedAtUtc;
        ExpiresAtUtc = expiresAtUtc;
        ApprovedAtUtc = null;
        RejectedAtUtc = null;
        CancelledAtUtc = null;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    public void Cancel(DateTime utcNow)
    {
        Status = RequestStatus.Cancelled;
        CancelledAtUtc = utcNow;
        UpdatedAtUtc = utcNow;
    }

    public void Approve(DateTime utcNow)
    {
        Status = RequestStatus.Approved;
        ApprovedAtUtc = utcNow;
        UpdatedAtUtc = utcNow;
    }

    public void Reject(DateTime utcNow)
    {
        Status = RequestStatus.Rejected;
        RejectedAtUtc = utcNow;
        UpdatedAtUtc = utcNow;
    }

    public void MarkInvoiced(DateTime utcNow)
    {
        IsInvoiced = true;
        UpdatedAtUtc = utcNow;
    }
}