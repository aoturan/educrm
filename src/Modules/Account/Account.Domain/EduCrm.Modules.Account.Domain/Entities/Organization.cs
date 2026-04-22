using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCrm.Modules.Account.Contracts.Enums;

namespace EduCrm.Modules.Account.Domain.Entities;


[Table("accounts_organizations")]
public sealed class Organization
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }
    
    [Required]
    [Column("name")]
    public string Name { get; private set; } = null!;
    
    [Required]
    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; private set; }
    
    
    [Column("updated_at_utc")]
    public DateTime? UpdatedAtUtc { get; private set; }
    
    [Column("plan_type")]
    public OrganizationPlanType PlanType { get; private set; }

    [Column("subscription_billing_cycle")]
    public SubscriptionBillingCycle SubscriptionBillingCycle { get; private set; }

    [Column("subscription_status")]
    public SubscriptionStatus SubscriptionStatus { get; private set; }

    [Column("subscription_started_at_utc")]
    public DateTime? SubscriptionStartedAtUtc { get; private set; }

    [Column("subscription_ends_at_utc")]
    public DateTime? SubscriptionEndsAtUtc { get; private set; }

    [Column("free_program_consumed_at_utc")]
    public DateTime? FreeProgramConsumedAtUtc { get; private set; }

    [Column("contact_name")]
    public string? ContactName { get; private set; }

    [Column("contact_email")]
    public string? ContactEmail { get; private set; }

    [Column("contact_phone")]
    public string? ContactPhone { get; private set; }

    public ICollection<User> Users { get; set; } = new List<User>();

    private Organization() { } // EF

    public Organization(Guid id, string name, DateTime createdAtUtc, string? contactName = null, string? contactEmail = null, string? contactPhone = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Organization id is required.", nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Organization name is required.", nameof(name));

        Id = id;
        Name = name.Trim();
        CreatedAtUtc = createdAtUtc;
        PlanType = OrganizationPlanType.Free;
        SubscriptionBillingCycle = SubscriptionBillingCycle.None;
        SubscriptionStatus = SubscriptionStatus.None;
        SubscriptionStartedAtUtc = null;
        SubscriptionEndsAtUtc = null;
        FreeProgramConsumedAtUtc = null;
        ContactName = string.IsNullOrWhiteSpace(contactName) ? null : contactName.Trim();
        ContactEmail = string.IsNullOrWhiteSpace(contactEmail) ? null : contactEmail.Trim();
        ContactPhone = string.IsNullOrWhiteSpace(contactPhone) ? null : contactPhone.Trim();
    }

    public void Rename(string name, DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Organization name is required.", nameof(name));

        Name = name.Trim();
        UpdatedAtUtc = utcNow;
    }

    public void ChangeContactInfo(string? contactName, string? contactEmail, string? contactPhone, DateTime utcNow)
    {
        ContactName = string.IsNullOrWhiteSpace(contactName) ? null : contactName.Trim();
        ContactEmail = string.IsNullOrWhiteSpace(contactEmail) ? null : contactEmail.Trim();
        ContactPhone = string.IsNullOrWhiteSpace(contactPhone) ? null : contactPhone.Trim();
        UpdatedAtUtc = utcNow;
    }

    public void MarkFreeProgramConsumed(DateTime utcNow)
    {
        if (utcNow == default) throw new ArgumentException("Current UTC time is required.", nameof(utcNow));

        FreeProgramConsumedAtUtc = utcNow;
        UpdatedAtUtc = utcNow;
    }
}