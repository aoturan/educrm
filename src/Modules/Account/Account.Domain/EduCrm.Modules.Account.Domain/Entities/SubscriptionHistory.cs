using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Domain.Entities;

[Table("accounts_organization_subscription_history")]
public sealed class SubscriptionHistory
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
    [Column("started_at_utc")]
    public DateTime StartedAtUtc { get; private set; }

    [Required]
    [Column("ended_at_utc")]
    public DateTime EndedAtUtc { get; private set; }

    [Column("amount")]
    public decimal? Amount { get; private set; }

    [Column("payment_method")]
    public PaymentMethod? PaymentMethod { get; private set; }

    [Column("payment_reference_code")]
    public string? PaymentReferenceCode { get; private set; }

    [Column("subscription_request_id")]
    public Guid? SubscriptionRequestId { get; private set; }

    [Required]
    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; private set; }

    private SubscriptionHistory() { } // EF

    public SubscriptionHistory(
        Guid id,
        Guid organizationId,
        PlanCode planCode,
        DateTime startedAtUtc,
        DateTime endedAtUtc,
        decimal? amount,
        PaymentMethod? paymentMethod,
        string? paymentReferenceCode,
        Guid? subscriptionRequestId,
        DateTime createdAtUtc)
    {
        if (id == Guid.Empty) throw new ArgumentException("SubscriptionHistory id is required.", nameof(id));
        if (organizationId == Guid.Empty) throw new ArgumentException("Organization is required.", nameof(organizationId));

        Id = id;
        OrganizationId = organizationId;
        PlanCode = planCode;
        StartedAtUtc = startedAtUtc;
        EndedAtUtc = endedAtUtc;
        Amount = amount;
        PaymentMethod = paymentMethod;
        PaymentReferenceCode = paymentReferenceCode;
        SubscriptionRequestId = subscriptionRequestId;
        CreatedAtUtc = createdAtUtc;
    }
}