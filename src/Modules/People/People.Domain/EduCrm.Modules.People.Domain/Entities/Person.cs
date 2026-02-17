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
    [Column("account_user_id")]
    public Guid AccountUserId { get; private set; } // FK -> accounts_organizations.id
    
    [Required]
    [Column("organization_id")]
    public Guid AccountOrganizationId { get; private set; } 

    // enum -> smallint conversion Fluent ile yapılacak
    [Required]
    [Column("type")]
    public PersonType Type { get; private set; }

    [Required]
    [Column("name_surname")]
    public string NameSurname { get; private set; } = default!;
    
    [Required]
    [Column("email")]
    public string Email { get; private set; } = default!;
    
    [Required]
    [Column("phone")]
    public string Phone { get; private set; } = default!;

    [Column("notes")]
    public string? Notes { get; private set; }
    
    // enum -> smallint conversion Fluent ile yapılacak
    [Required]
    [Column("status")]
    public PersonStatus Status { get; private set; }

    [Required]
    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; private set; }
    
    [Column("updated_at_utc")]
    public DateTime? UpdatedAtUtc { get; private set; }
    
    private Person() { } // EF

    public Person(
        Guid id,
        Guid accountOrganizationId,
        Guid userId,
        PersonType type,
        string name,
        string email,
        string phone,
        DateTime createdAtUtc)
    {
        if (id == Guid.Empty) throw new ArgumentException("Person id is required.", nameof(id));
        if (accountOrganizationId == Guid.Empty) throw new ArgumentException("Organization is required.", nameof(accountOrganizationId));
        if (userId == Guid.Empty) throw new ArgumentException("UserID is required.", nameof(userId));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));
        if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("Phone is required.", nameof(phone));

        Id = id;
        AccountOrganizationId = accountOrganizationId;
        AccountUserId = userId;
        Type = type;

        NameSurname = name.Trim();
        Email = email.Trim();
        Phone = phone.Trim();

        Status = PersonStatus.Active;
        CreatedAtUtc = createdAtUtc;
    }

    public void UpdateContact(string email, string phone, DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));
        if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("Phone is required.", nameof(phone));

        Email = email.Trim();
        Phone = phone.Trim();
        UpdatedAtUtc = utcNow;
    }

    public void UpdateNameSurname(string nameSurname, DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(nameSurname)) throw new ArgumentException("Name is required.", nameof(nameSurname));

        NameSurname = nameSurname.Trim();
        UpdatedAtUtc = utcNow;
    }

    public void UpdateEmail(string email, DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));

        Email = email.Trim();
        UpdatedAtUtc = utcNow;
    }

    public void SetNotes(string? notes, DateTime utcNow)
    {
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        UpdatedAtUtc = utcNow;
    }

    public void Archive(DateTime utcNow)
    {
        Status = PersonStatus.Archived;
        UpdatedAtUtc = utcNow;
    }

    public void Activate(DateTime utcNow)
    {
        Status = PersonStatus.Active;
        UpdatedAtUtc = utcNow;
    }
}
