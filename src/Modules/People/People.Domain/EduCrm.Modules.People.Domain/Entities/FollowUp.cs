using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.Modules.People.Domain.Entities;

[Table("person_follow_ups")]
public class FollowUp
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }

    [Required]
    [Column("organization_id")]
    public Guid OrganizationId { get; private set; }

    [Required]
    [Column("person_id")]
    public Guid PersonId { get; private set; }

    [Column("program_id")]
    public Guid? ProgramId { get; private set; }

    [Required]
    [Column("type")]
    public FollowUpType Type { get; private set; }

    [Required]
    [Column("status")]
    public FollowUpStatus Status { get; private set; }

    [Required]
    [Column("title")]
    public string Title { get; private set; } = null!;

    [Column("note")]
    public string? Note { get; private set; }

    [Required]
    [Column("due_at_utc")]
    public DateTime DueAtUtc { get; private set; }

    [Column("snoozed_until_utc")]
    public DateTime? SnoozedUntilUtc { get; private set; }

    [Column("completed_at_utc")]
    public DateTime? CompletedAtUtc { get; private set; }

    [Column("cancelled_at_utc")]
    public DateTime? CancelledAtUtc { get; private set; }

    [Required]
    [Column("created_by_user_id")]
    public Guid CreatedByUserId { get; private set; }

    [Required]
    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; private set; }

    [Column("updated_at_utc")]
    public DateTime? UpdatedAtUtc { get; private set; }

    private FollowUp() { } // EF Core

    public FollowUp(
        Guid organizationId,
        Guid personId,
        Guid createdByUserId,
        FollowUpType type,
        string title,
        DateTime dueAtUtc,
        DateTime createdAtUtc,
        Guid? programId = null,
        string? note = null)
    {
        if (organizationId == Guid.Empty)
            throw new ArgumentException("Organization ID cannot be empty.", nameof(organizationId));

        if (personId == Guid.Empty)
            throw new ArgumentException("Person ID cannot be empty.", nameof(personId));

        if (createdByUserId == Guid.Empty)
            throw new ArgumentException("Created by user ID cannot be empty.", nameof(createdByUserId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));

        if (title.Length > 200)
            throw new ArgumentException("Title cannot exceed 200 characters.", nameof(title));

        if (dueAtUtc == default)
            throw new ArgumentException("Due date is required.", nameof(dueAtUtc));

        if (createdAtUtc == default)
            throw new ArgumentException("Created at UTC is required.", nameof(createdAtUtc));

        Id = Guid.NewGuid();
        OrganizationId = organizationId;
        PersonId = personId;
        ProgramId = programId;
        CreatedByUserId = createdByUserId;
        Type = type;
        Status = FollowUpStatus.Open;
        Title = title.Trim();
        Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
        DueAtUtc = dueAtUtc;
        SnoozedUntilUtc = null;
        CompletedAtUtc = null;
        CancelledAtUtc = null;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = null;
    }

    public void Update(
        Guid personId,
        Guid? programId,
        FollowUpType type,
        string title,
        string? note,
        DateTime utcNow)
    {
        if (personId == Guid.Empty)
            throw new ArgumentException("Person ID cannot be empty.", nameof(personId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));

        if (title.Length > 200)
            throw new ArgumentException("Title cannot exceed 200 characters.", nameof(title));

        PersonId = personId;
        ProgramId = programId;
        Type = type;
        Title = title.Trim();
        Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
        UpdatedAtUtc = utcNow;
    }

    public void Snooze(DateTime snoozeUntilUtc, DateTime utcNow)
    {
        if (snoozeUntilUtc == default)
            throw new ArgumentException("Snooze until UTC is required.", nameof(snoozeUntilUtc));

        if (snoozeUntilUtc <= DueAtUtc)
            throw new ArgumentException("Snooze until UTC must be after the due date.", nameof(snoozeUntilUtc));

        Status = FollowUpStatus.Snoozed;
        SnoozedUntilUtc = snoozeUntilUtc;
        UpdatedAtUtc = utcNow;
    }

    public void RescheduleDueDate(DateTime newDueAtUtc, DateTime utcNow)
    {
        if (newDueAtUtc == default)
            throw new ArgumentException("New due date is required.", nameof(newDueAtUtc));

        DueAtUtc = newDueAtUtc;
        Status = FollowUpStatus.Open;
        SnoozedUntilUtc = null;
        UpdatedAtUtc = utcNow;
    }

    public void Complete(DateTime utcNow)
    {
        if (Status == FollowUpStatus.Completed)
            throw new InvalidOperationException("Follow-up is already completed.");

        if (Status == FollowUpStatus.Cancelled)
            throw new InvalidOperationException("A cancelled follow-up cannot be completed.");

        Status = FollowUpStatus.Completed;
        CompletedAtUtc = utcNow;
        UpdatedAtUtc = utcNow;
    }

    public void Cancel(DateTime utcNow)
    {
        if (Status == FollowUpStatus.Cancelled)
            throw new InvalidOperationException("Follow-up is already cancelled.");

        if (Status == FollowUpStatus.Completed)
            throw new InvalidOperationException("A completed follow-up cannot be cancelled.");

        Status = FollowUpStatus.Cancelled;
        CancelledAtUtc = utcNow;
        UpdatedAtUtc = utcNow;
    }
}