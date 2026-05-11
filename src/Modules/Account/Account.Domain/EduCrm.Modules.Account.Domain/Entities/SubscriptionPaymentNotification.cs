using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduCrm.Modules.Account.Domain.Entities;

[Table("accounts_subscription_payment_notifications")]
public sealed class SubscriptionPaymentNotification
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }

    [Required]
    [Column("subscription_request_id")]
    public Guid SubscriptionRequestId { get; private set; }

    [Required]
    [Column("organization_id")]
    public Guid OrganizationId { get; private set; }

    [Required]
    [Column("sender_name")]
    [StringLength(200)]
    public string SenderName { get; private set; } = string.Empty;

    [Required]
    [Column("payment_date")]
    public DateOnly PaymentDate { get; private set; }

    [Required]
    [Column("amount")]
    public decimal Amount { get; private set; }

    [Column("note")]
    [StringLength(500)]
    public string? Note { get; private set; }

    [Column("receipt_object_key")]
    [StringLength(500)]
    public string? ReceiptObjectKey { get; private set; }

    [Column("receipt_file_name")]
    [StringLength(255)]
    public string? ReceiptFileName { get; private set; }

    [Column("receipt_content_type")]
    [StringLength(100)]
    public string? ReceiptContentType { get; private set; }

    [Column("receipt_size_bytes")]
    public long? ReceiptSizeBytes { get; private set; }

    [Required]
    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; private set; }

    [Required]
    [Column("updated_at_utc")]
    public DateTime UpdatedAtUtc { get; private set; }

    private SubscriptionPaymentNotification() { } // EF

    public SubscriptionPaymentNotification(
        Guid id,
        Guid subscriptionRequestId,
        Guid organizationId,
        string senderName,
        DateOnly paymentDate,
        decimal amount,
        string? note,
        string? receiptObjectKey,
        string? receiptFileName,
        string? receiptContentType,
        long? receiptSizeBytes,
        DateTime createdAtUtc)
    {
        if (id == Guid.Empty) throw new ArgumentException("SubscriptionPaymentNotification id is required.", nameof(id));
        if (subscriptionRequestId == Guid.Empty) throw new ArgumentException("Subscription request is required.", nameof(subscriptionRequestId));
        if (organizationId == Guid.Empty) throw new ArgumentException("Organization is required.", nameof(organizationId));

        Id = id;
        SubscriptionRequestId = subscriptionRequestId;
        OrganizationId = organizationId;
        SenderName = senderName;
        PaymentDate = paymentDate;
        Amount = amount;
        Note = note;
        ReceiptObjectKey = receiptObjectKey;
        ReceiptFileName = receiptFileName;
        ReceiptContentType = receiptContentType;
        ReceiptSizeBytes = receiptSizeBytes;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }
}