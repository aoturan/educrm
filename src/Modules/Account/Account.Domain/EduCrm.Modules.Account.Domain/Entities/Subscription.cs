using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Domain.Entities;

[Table("accounts_subscriptions")]
public sealed class Subscription
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }

    [Required]
    [Column("organization_id")]
    public Guid OrganizationId { get; private set; }

    [Required]
    [Column("plan_code")]
    public PlanCode PlanCode { get; private set; }

    [Required]
    [Column("starts_at_utc")]
    public DateTime StartsAtUtc { get; private set; }

    [Column("ends_at_utc")]
    public DateTime? EndsAtUtc { get; private set; }

    [Required]
    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; private set; }

    [Required]
    [Column("updated_at_utc")]
    public DateTime UpdatedAtUtc { get; private set; }

    [Column("downgraded_from_plan_code")]
    public PlanCode? DowngradedFromPlanCode { get; private set; }

    [Column("downgraded_at_utc")]
    public DateTime? DowngradedAtUtc { get; private set; }

    [Column("activated_by_subscription_request_id")]
    public Guid? ActivatedBySubscriptionRequestId { get; private set; }

    private Subscription() { } // EF

    public Subscription(
        Guid id,
        Guid organizationId,
        PlanCode planCode,
        DateTime startsAtUtc,
        DateTime? endsAtUtc,
        DateTime createdAtUtc)
    {
        if (id == Guid.Empty) throw new ArgumentException("Subscription id is required.", nameof(id));
        if (organizationId == Guid.Empty) throw new ArgumentException("Organization is required.", nameof(organizationId));
        if (endsAtUtc is not null && endsAtUtc < startsAtUtc)
            throw new ArgumentException("EndsAtUtc cannot be earlier than StartsAtUtc.", nameof(endsAtUtc));

        Id = id;
        OrganizationId = organizationId;
        PlanCode = planCode;
        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    public void ChangePlan(PlanCode planCode, DateTime utcNow)
    {
        PlanCode = planCode;
        UpdatedAtUtc = utcNow;
    }

    public void SetEndsAt(DateTime? endsAtUtc, DateTime utcNow)
    {
        if (endsAtUtc is not null && endsAtUtc < StartsAtUtc)
            throw new ArgumentException("EndsAtUtc cannot be earlier than StartsAtUtc.", nameof(endsAtUtc));

        EndsAtUtc = endsAtUtc;
        UpdatedAtUtc = utcNow;
    }

    public void DowngradeToFree(DateTime utcNow)
    {
        if (PlanCode == PlanCode.Free) return;

        DowngradedFromPlanCode = PlanCode;
        DowngradedAtUtc = utcNow;
        PlanCode = PlanCode.Free;
        EndsAtUtc = null;
        UpdatedAtUtc = utcNow;
    }

    public void MarkActivatedBy(Guid subscriptionRequestId, DateTime utcNow)
    {
        if (subscriptionRequestId == Guid.Empty)
            throw new ArgumentException("SubscriptionRequest id is required.", nameof(subscriptionRequestId));

        ActivatedBySubscriptionRequestId = subscriptionRequestId;
        UpdatedAtUtc = utcNow;
    }

    public void ActivateFromRequest(
        PlanCode planCode,
        DateTime startsAtUtc,
        DateTime endsAtUtc,
        Guid subscriptionRequestId,
        DateTime utcNow)
    {
        if (subscriptionRequestId == Guid.Empty)
            throw new ArgumentException("SubscriptionRequest id is required.", nameof(subscriptionRequestId));
        if (endsAtUtc <= startsAtUtc)
            throw new ArgumentException("EndsAtUtc must be after StartsAtUtc.", nameof(endsAtUtc));

        PlanCode = planCode;
        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;
        DowngradedFromPlanCode = null;
        DowngradedAtUtc = null;
        ActivatedBySubscriptionRequestId = subscriptionRequestId;
        UpdatedAtUtc = utcNow;
    }

    public void OverrideByAdmin(
        PlanCode planCode,
        DateTime startsAtUtc,
        DateTime endsAtUtc,
        DateTime utcNow)
    {
        if (endsAtUtc <= startsAtUtc)
            throw new ArgumentException("EndsAtUtc must be after StartsAtUtc.", nameof(endsAtUtc));

        PlanCode = planCode;
        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;
        DowngradedFromPlanCode = null;
        DowngradedAtUtc = null;
        ActivatedBySubscriptionRequestId = null;
        UpdatedAtUtc = utcNow;
    }
}
