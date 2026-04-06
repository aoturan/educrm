using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduCrm.Modules.Program.Domain.Entities;

[Table("program_enrollments")]
public sealed class Enrollment
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

    [Required]
    [Column("person_id")]
    public Guid PersonId { get; private set; }

    [Required]
    [Column("enrolled_at_utc")]
    public DateTime EnrolledAtUtc { get; private set; }

    private Enrollment()
    {
    }

    public Enrollment(
        Guid organizationId,
        Guid programId,
        Guid personId,
        DateTime enrolledAtUtc)
    {
        if (organizationId == Guid.Empty)
            throw new ArgumentException("Organization ID cannot be empty.", nameof(organizationId));

        if (programId == Guid.Empty)
            throw new ArgumentException("Program ID cannot be empty.", nameof(programId));

        if (personId == Guid.Empty)
            throw new ArgumentException("Person ID cannot be empty.", nameof(personId));

        if (enrolledAtUtc == default)
            throw new ArgumentException("EnrolledAtUtc is required.", nameof(enrolledAtUtc));

        Id = Guid.NewGuid();
        OrganizationId = organizationId;
        ProgramId = programId;
        PersonId = personId;
        EnrolledAtUtc = enrolledAtUtc;
    }
}

