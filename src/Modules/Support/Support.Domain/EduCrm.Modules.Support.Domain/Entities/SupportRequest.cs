using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCrm.Modules.Support.Domain.Enums;

namespace EduCrm.Modules.Support.Domain.Entities;

[Table("support_requests")]
public sealed class SupportRequest
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }

    [Required]
    [Column("organization_id")]
    public Guid OrganizationId { get; private set; }

    [Required]
    [Column("user_id")]
    public Guid UserId { get; private set; }

    [Required]
    [Column("subject")]
    public string Subject { get; private set; } = null!;

    [Required]
    [Column("message")]
    public string Message { get; private set; } = null!;

    [Column("page_url")]
    public string? PageUrl { get; private set; }

    [Required]
    [Column("status")]
    public SupportRequestStatus Status { get; private set; }

    [Required]
    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; private set; }

    [Column("handled_at_utc")]
    public DateTime? HandledAtUtc { get; private set; }

    private SupportRequest() { } // EF Core

    public SupportRequest(
        Guid organizationId,
        Guid userId,
        string subject,
        string message,
        DateTime createdAtUtc,
        string? pageUrl = null)
    {
        if (organizationId == Guid.Empty)
            throw new ArgumentException("Organization ID cannot be empty.", nameof(organizationId));

        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Subject cannot be empty.", nameof(subject));

        if (subject.Length > 150)
            throw new ArgumentException("Subject cannot exceed 150 characters.", nameof(subject));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty.", nameof(message));

        if (message.Length > 4000)
            throw new ArgumentException("Message cannot exceed 4000 characters.", nameof(message));

        if (pageUrl is { Length: > 500 })
            throw new ArgumentException("Page URL cannot exceed 500 characters.", nameof(pageUrl));

        Id = Guid.NewGuid();
        OrganizationId = organizationId;
        UserId = userId;
        Subject = subject.Trim();
        Message = message.Trim();
        PageUrl = string.IsNullOrWhiteSpace(pageUrl) ? null : pageUrl.Trim();
        Status = SupportRequestStatus.New;
        CreatedAtUtc = createdAtUtc;
        HandledAtUtc = null;
    }
}