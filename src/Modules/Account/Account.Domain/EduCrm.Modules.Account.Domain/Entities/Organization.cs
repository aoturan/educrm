using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduCrm.Modules.Account.Domain.Entities;


[Table("accounts_organizations")]
public sealed class Organization
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }
    
    [Required]
    [Column("name")]
    public string Name { get; private set; } = null!;
    
    [Required]
    [Column("created_at_utc")]
    public DateTime CreatedAtUtc { get; private set; }
    
    
    [Column("updated_at_utc")]
    public DateTime? UpdatedAtUtc { get; private set; }
    
    public ICollection<User> Users { get; set; } = new List<User>();

    private Organization() { } // EF

    public Organization(Guid id, string name, DateTime createdAtUtc)
    {
        if (id == Guid.Empty) throw new ArgumentException("Organization id is required.", nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Organization name is required.", nameof(name));

        Id = id;
        Name = name.Trim();
        CreatedAtUtc = createdAtUtc;
    }

    public void Rename(string name, DateTime utcNow)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Organization name is required.", nameof(name));

        Name = name.Trim();
        UpdatedAtUtc = utcNow;
    }
}