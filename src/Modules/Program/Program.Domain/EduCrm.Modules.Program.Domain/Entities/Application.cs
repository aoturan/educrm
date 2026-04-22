using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Domain.Entities;

[Table("program_applications")]
public sealed class Application
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }

    [Required]
    [Column("organization_id")]
    public Guid OrganizationId { get; private set; }

    [Required]
    [Column("program_id")]
    public Guid ProgramId { get; private set; }

    [Column("person_id")]
    public Guid? PersonId { get; private set; }

    [Required]
    [Column("status")]
    public ApplicationStatus Status { get; private set; }


    [Required]
    [Column("submitted_full_name")]
    public string SubmittedFullName { get; private set; } = default!;

    [Required]
    [Column("submitted_phone")]
    public string SubmittedPhone { get; private set; } = default!;

    [Required]
    [Column("submitted_email")]
    public string SubmittedEmail { get; private set; } = default!;

    [Required]
    [Column("submitted_message")]
    public string SubmittedMessage { get; private set; } = default!;

    [Required]
    [Column("first_submitted_at_utc")]
    public DateTime FirstSubmittedAtUtc { get; private set; }

    [Required]
    [Column("last_submitted_at_utc")]
    public DateTime LastSubmittedAtUtc { get; private set; }

    [Required]
    [Column("submission_count")]
    public int SubmissionCount { get; private set; }

    [Column("converted_enrollment_id")]
    public Guid? ConvertedEnrollmentId { get; private set; }

    [Column("converted_at_utc")]
    public DateTime? ConvertedAtUtc { get; private set; }

    [Column("closed_at_utc")]
    public DateTime? ClosedAtUtc { get; private set; }

    [Column("closed_note")]
    public string? ClosedNote { get; private set; }

    [Required]
    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; private set; }

    [Required]
    [Column("updated_at_utc")]
    public DateTime UpdatedAtUtc { get; private set; }

    private Application() { } // EF Core

    public Application(
        Guid organizationId,
        Guid programId,
        Guid? personId,
        DateTime nowUtc,
        string submittedFullName,
        string submittedPhone,
        string submittedEmail,
        string submittedMessage)
    {
        if (organizationId == Guid.Empty)
            throw new ArgumentException("Organization ID cannot be empty.", nameof(organizationId));

        if (programId == Guid.Empty)
            throw new ArgumentException("Program ID cannot be empty.", nameof(programId));

        if (personId.HasValue && personId.Value == Guid.Empty)
            throw new ArgumentException("Person ID cannot be empty.", nameof(personId));

        Id = Guid.NewGuid();
        OrganizationId = organizationId;
        ProgramId = programId;
        PersonId = personId;
        Status = ApplicationStatus.New;
        SubmittedFullName = submittedFullName;
        SubmittedPhone = submittedPhone;
        SubmittedEmail = submittedEmail;
        SubmittedMessage = submittedMessage;
        FirstSubmittedAtUtc = nowUtc;
        LastSubmittedAtUtc = nowUtc;
        SubmissionCount = 1;
        ConvertedEnrollmentId = null;
        ConvertedAtUtc = null;
        ClosedAtUtc = null;
        CreatedAtUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
    }

    public void IncrementSubmission(DateTime nowUtc)
    {
        SubmissionCount++;
        LastSubmittedAtUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
    }

    public void AssignPerson(Guid personId, DateTime nowUtc)
    {
        if (personId == Guid.Empty)
            throw new ArgumentException("Person ID cannot be empty.", nameof(personId));

        PersonId = personId;
        UpdatedAtUtc = nowUtc;
    }

    public void MarkContacted(Guid personId, DateTime nowUtc)
    {
        if (personId == Guid.Empty)
            throw new ArgumentException("Person ID cannot be empty.", nameof(personId));

        PersonId = personId;
        Status = ApplicationStatus.Contacted;
        UpdatedAtUtc = nowUtc;
    }

    public void MarkContactedStatus(DateTime nowUtc)
    {
        Status = ApplicationStatus.Contacted;
        UpdatedAtUtc = nowUtc;
    }

    public void Close(string? closedNote, DateTime nowUtc)
    {
        Status = ApplicationStatus.Closed;
        ClosedNote = closedNote;
        ClosedAtUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
    }

    public void MarkConverted(Guid enrollmentId, DateTime nowUtc)
    {
        if (enrollmentId == Guid.Empty)
            throw new ArgumentException("Enrollment ID cannot be empty.", nameof(enrollmentId));

        Status = ApplicationStatus.Converted;
        ConvertedEnrollmentId = enrollmentId;
        ConvertedAtUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
    }
}