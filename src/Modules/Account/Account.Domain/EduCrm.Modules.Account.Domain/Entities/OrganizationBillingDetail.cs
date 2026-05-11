using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Domain.Entities;

[Table("accounts_organization_billing_details")]
public sealed class OrganizationBillingDetail
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }

    [Required]
    [Column("organization_id")]
    public Guid OrganizationId { get; private set; }

    [Required]
    [Column("billing_type")]
    public BillingType BillingType { get; private set; }

    [Required]
    [Column("billing_name")]
    [StringLength(200)]
    public string BillingName { get; private set; } = null!;

    [Column("tax_number")]
    [StringLength(20)]
    public string? TaxNumber { get; private set; }

    [Column("tax_office")]
    [StringLength(100)]
    public string? TaxOffice { get; private set; }

    [Required]
    [Column("billing_email")]
    [StringLength(320)]
    public string BillingEmail { get; private set; } = null!;

    [Required]
    [Column("billing_address")]
    [StringLength(500)]
    public string BillingAddress { get; private set; } = null!;

    [Required]
    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; private set; }

    [Required]
    [Column("updated_at_utc")]
    public DateTime UpdatedAtUtc { get; private set; }

    private OrganizationBillingDetail() { } // EF

    public OrganizationBillingDetail(
        Guid id,
        Guid organizationId,
        BillingType billingType,
        string billingName,
        string? taxNumber,
        string? taxOffice,
        string billingEmail,
        string billingAddress,
        DateTime createdAtUtc)
    {
        if (id == Guid.Empty) throw new ArgumentException("OrganizationBillingDetail id is required.", nameof(id));
        if (organizationId == Guid.Empty) throw new ArgumentException("Organization is required.", nameof(organizationId));
        if (string.IsNullOrWhiteSpace(billingName)) throw new ArgumentException("BillingName is required.", nameof(billingName));
        if (string.IsNullOrWhiteSpace(billingEmail)) throw new ArgumentException("BillingEmail is required.", nameof(billingEmail));
        if (string.IsNullOrWhiteSpace(billingAddress)) throw new ArgumentException("BillingAddress is required.", nameof(billingAddress));

        Id = id;
        OrganizationId = organizationId;
        BillingType = billingType;
        BillingName = billingName.Trim();
        TaxNumber = string.IsNullOrWhiteSpace(taxNumber) ? null : taxNumber.Trim();
        TaxOffice = string.IsNullOrWhiteSpace(taxOffice) ? null : taxOffice.Trim();
        BillingEmail = billingEmail.Trim();
        BillingAddress = billingAddress.Trim();
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    public void Update(
        BillingType billingType,
        string billingName,
        string? taxNumber,
        string? taxOffice,
        string billingEmail,
        string billingAddress,
        DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(billingName)) throw new ArgumentException("BillingName is required.", nameof(billingName));
        if (string.IsNullOrWhiteSpace(billingEmail)) throw new ArgumentException("BillingEmail is required.", nameof(billingEmail));
        if (string.IsNullOrWhiteSpace(billingAddress)) throw new ArgumentException("BillingAddress is required.", nameof(billingAddress));

        BillingType = billingType;
        BillingName = billingName.Trim();
        TaxNumber = string.IsNullOrWhiteSpace(taxNumber) ? null : taxNumber.Trim();
        TaxOffice = string.IsNullOrWhiteSpace(taxOffice) ? null : taxOffice.Trim();
        BillingEmail = billingEmail.Trim();
        BillingAddress = billingAddress.Trim();
        UpdatedAtUtc = utcNow;
    }
}