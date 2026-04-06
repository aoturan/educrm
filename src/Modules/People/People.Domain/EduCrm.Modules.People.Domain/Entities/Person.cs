using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCrm.Modules.People.Domain.Enums;

namespace EduCrm.Modules.People.Domain.Entities;

[Table("people_persons")]
public sealed class Person
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }

    [Required]
    [Column("organization_id")]
    public Guid OrganizationId { get; private set; }

    [Required]
    [Column("full_name")]
    public string FullName { get; private set; } = null!;

    [Column("phone")]
    public string? Phone { get; private set; }

    [Column("email")]
    public string? Email { get; private set; }

    [Column("notes")]
    public string? Notes { get; private set; }

    [Required]
    [Column("source")]
    public SourceType Source { get; private set; }

    [Required]
    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; private set; }

    [Required]
    [Column("updated_at_utc")]
    public DateTime UpdatedAtUtc { get; private set; }

    [Required]
    [Column("is_archived")]
    public bool IsArchived { get; private set; }

    [Column("archived_at_utc")]
    public DateTime? ArchivedAtUtc { get; private set; }

    private Person()
    {
    }

    public Person(
        Guid organizationId,
        string fullName,
        SourceType source,
        DateTime nowUtc,
        string? phone = null,
        string? email = null,
        string? notes = null)
    {
        if (organizationId == Guid.Empty)
            throw new ArgumentException("Organization ID cannot be empty.", nameof(organizationId));

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be null or empty.", nameof(fullName));

        if (!Enum.IsDefined(typeof(SourceType), source))
            throw new ArgumentException("Source must be a valid value.", nameof(source));

        if (nowUtc == default)
            throw new ArgumentException("Current UTC time is required.", nameof(nowUtc));

        Id = Guid.NewGuid();
        OrganizationId = organizationId;
        FullName = fullName.Trim();
        Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        Source = source;
        CreatedAtUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
        IsArchived = false;
        ArchivedAtUtc = null;
    }

    public void Archive(DateTime utcNow)
    {
        if (IsArchived)
            throw new InvalidOperationException("Person is already archived.");

        IsArchived = true;
        ArchivedAtUtc = utcNow;
        UpdatedAtUtc = utcNow;
    }

    public void Unarchive(DateTime utcNow)
    {
        if (!IsArchived)
            throw new InvalidOperationException("Person is not archived.");

        IsArchived = false;
        ArchivedAtUtc = null;
        UpdatedAtUtc = utcNow;
    }

    public void Update(
        string fullName,
        string? phone,
        string? email,
        string? notes,
        DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be null or empty.", nameof(fullName));

        FullName = fullName.Trim();
        Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        UpdatedAtUtc = utcNow;
    }
}