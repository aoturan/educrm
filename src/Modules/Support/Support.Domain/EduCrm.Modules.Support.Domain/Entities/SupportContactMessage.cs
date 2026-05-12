using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCrm.Modules.Support.Domain.Enums;

namespace EduCrm.Modules.Support.Domain.Entities;

[Table("support_contact_messages")]
public sealed class SupportContactMessage
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }

    [Required]
    [Column("full_name")]
    public string FullName { get; private set; } = null!;

    [Required]
    [Column("email")]
    public string Email { get; private set; } = null!;

    [Required]
    [Column("subject")]
    public string Subject { get; private set; } = null!;

    [Required]
    [Column("message")]
    public string Message { get; private set; } = null!;

    [Required]
    [Column("status")]
    public SupportContactMessageStatus Status { get; private set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; private set; }

    [Column("reviewed_at")]
    public DateTime? ReviewedAt { get; private set; }

    private SupportContactMessage() { } // EF Core

    public SupportContactMessage(
        string fullName,
        string email,
        string subject,
        string message,
        DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty.", nameof(fullName));

        if (fullName.Length > 200)
            throw new ArgumentException("Full name cannot exceed 200 characters.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        if (email.Length > 320)
            throw new ArgumentException("Email cannot exceed 320 characters.", nameof(email));

        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Subject cannot be empty.", nameof(subject));

        if (subject.Length > 200)
            throw new ArgumentException("Subject cannot exceed 200 characters.", nameof(subject));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty.", nameof(message));

        Id = Guid.NewGuid();
        FullName = fullName.Trim();
        Email = email.Trim();
        Subject = subject.Trim();
        Message = message.Trim();
        Status = SupportContactMessageStatus.New;
        CreatedAt = createdAt;
        ReviewedAt = null;
    }

    public void MarkReviewed(DateTime reviewedAt)
    {
        Status = SupportContactMessageStatus.Handled;
        ReviewedAt = reviewedAt;
    }
}
