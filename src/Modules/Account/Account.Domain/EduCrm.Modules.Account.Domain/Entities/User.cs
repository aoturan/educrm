using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCrm.Modules.Account.Domain.Enums;

namespace EduCrm.Modules.Account.Domain.Entities;

[Table("accounts_users")]
public sealed class User
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }
    
    [Required]
    [Column("organization_id")]
    public Guid OrganizationId { get; private set; } // FK -> accounts_organizations.id

    [Required]
    [Column("email")]
    public string Email { get; private set; } = null!;
    
    [Required]
    [Column("full_name")]
    public string FullName { get; private set; } = null!;
    
    [Column("phone")]
    public string? Phone { get; private set; }
    
    [Required]
    [Column("password_hash")]
    public string PasswordHash { get; private set; } = null!;

    // enum -> smallint conversion Fluent ile yapılacak
    [Required]
    [Column("status")]
    public UserStatus Status { get; private set; }
    
    [Required]
    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; private set; }
    
    [Column("updated_at_utc")]
    public DateTime? UpdatedAtUtc { get; private set; }
    
    
    [Column("last_login_at_utc")]
    public DateTime? LastLoginAtUtc { get; private set; }
    
    public Organization Organization { get; set; } = null!;

    private User() { } // EF

    public User(Guid id, Guid organizationId, string email, string fullName, string? phone, string passwordHash, DateTime createdAtUtc)
    {
        if (id == Guid.Empty) throw new ArgumentException("User id is required.", nameof(id));
        if (organizationId == Guid.Empty) throw new ArgumentException("Organization is required.", nameof(organizationId));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("FullName is required.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("PasswordHash is required.", nameof(passwordHash));

        Id = id;
        OrganizationId = organizationId;
        Email = email.Trim();
        FullName = fullName.Trim();
        Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
        PasswordHash = passwordHash;
        Status = UserStatus.WaitingForActivation;
        CreatedAtUtc = createdAtUtc;
    }

    public void Disable(DateTime utcNow)
    {
        Status = UserStatus.Disabled;
        UpdatedAtUtc = utcNow;
    }

    public void Enable(DateTime utcNow)
    {
        Status = UserStatus.Active;
        UpdatedAtUtc = utcNow;
    }

    public void SetLastLogin(DateTime utcNow)
    {
        LastLoginAtUtc = utcNow;
        UpdatedAtUtc = utcNow;
    }

    public void ChangeEmail(string email, DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));

        Email = email.Trim();
        UpdatedAtUtc = utcNow;
    }

    public void ChangeFullName(string fullName, DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("FullName is required.", nameof(fullName));

        FullName = fullName.Trim();
        UpdatedAtUtc = utcNow;
    }

    public void ChangePhone(string? phone, DateTime utcNow)
    {
        Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
        UpdatedAtUtc = utcNow;
    }

    public void ChangePasswordHash(string passwordHash, DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("PasswordHash is required.", nameof(passwordHash));

        PasswordHash = passwordHash;
        UpdatedAtUtc = utcNow;
    }
}