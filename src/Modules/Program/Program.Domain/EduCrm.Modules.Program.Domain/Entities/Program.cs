using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EduCrm.Modules.Program.Domain.Enums;

namespace EduCrm.Modules.Program.Domain.Entities;

[Table("program_programs")]
public class Program
{
    [Key]
    [Column("id")]
    public Guid Id { get; private set; }
    
    [Required]
    [Column("organization_id")]
    public Guid OrganizationId { get; private set; }

    [Required]
    [Column("created_by_user_id")]
    public Guid CreatedByUserId { get; private set; }

    [Required] [Column("name")] public string Name { get; private set; } = null!;
    
    [Required]
    [Column("start_date")]
    public DateOnly StartDate { get; private set; }
    
    [Required]
    [Column("end_date")]
    public DateOnly EndDate { get; private set; }
    
    [Column("internal_notes")]
    public string? InternalNotes { get; private set; }
    
    [Required]
    [Column("public_short_description")]
    public string PublicShortDescription { get; private set; }  = null!;
    
    [Column("public_detailed_description")]
    public string? PublicDetailedDescription { get; private set; }
    
    [Required]
    [Column("public_modality")]
    public ProgramModality PublicModality { get; private set; }
    
    [Column("location_details")]
    public string? LocationDetails { get; private set; }

    [Column("online_participation_info")]
    public string? OnlineParticipationInfo { get; private set; }

    [Column("capacity")]
    public int? Capacity { get; private set; }

    [Required]
    [Column("public_schedule_text")]
    public string PublicScheduleText { get; private set; }  = null!;
    
    [Column("public_instructor_name")]
    public string? PublicInstructorName { get; private set; }
    
    [Column("public_enrollment_deadline")]
    public DateOnly? PublicEnrollmentDeadline { get; private set; }

    [Column("is_public")]
    public bool IsPublic { get; private set; }

    [Column("public_slug")]
    public string? PublicSlug { get; private set; }

    [Column("public_published_at_utc")]
    public DateTime? PublicPublishedAtUtc { get; private set; }

    [Column("price_amount")]
    public int? PriceAmount { get; private set; }

    [Column("price_currency")]
    public PriceCurrency? PriceCurrency { get; private set; }

    [Column("price_note")]
    public string? PriceNote { get; private set; }

    [Column("is_archived")]
    public bool IsArchived { get; private set; }

    [Column("archived_at_utc")]
    public DateTime? ArchivedAtUtc { get; private set; }

    [Column("status")]
    public ProgramStatus Status { get; private set; }
    
    [Column("completed_at")]
    public DateTime? CompletedAtUtc { get; private set; }
    
    [Column("created_at")]
    public DateTime CreatedAtUtc { get; private set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAtUtc { get; private set; }
    
    private Program()
    {
    }  // ef için

    public Program(
        Guid organizationId,
        Guid createdByUserId,
        string name,
        DateOnly startDate,
        DateOnly endDate,
        string publicShortDescription,
        ProgramModality publicModality,
        string publicScheduleText,
        DateOnly today,
        DateTime nowUtc,
        string? internalNotes = null,
        string? publicDetailedDescription = null,
        string? locationDetails = null,
        string? onlineParticipationInfo = null,
        int? capacity = null,
        string? publicInstructorName = null,
        DateOnly? publicEnrollmentDeadline = null,
        int? priceAmount = null,
        PriceCurrency? priceCurrency = null,
        string? priceNote = null)
    {
        if (organizationId == Guid.Empty)
            throw new ArgumentException("Organization ID cannot be empty.", nameof(organizationId));
        
        if (createdByUserId == Guid.Empty)
            throw new ArgumentException("Created user ID cannot be empty.", nameof(createdByUserId));
        
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        
        if (string.IsNullOrWhiteSpace(publicShortDescription))
            throw new ArgumentException("Public short description cannot be null or empty.", nameof(publicShortDescription));
        
        if (string.IsNullOrWhiteSpace(publicScheduleText))
            throw new ArgumentException("Public schedule text cannot be null or empty.", nameof(publicScheduleText));
        
        if (!Enum.IsDefined(typeof(ProgramModality), publicModality))
            throw new ArgumentException("Program Modality cannot be null or empty.", nameof(publicModality));
        
        if (today == default)
            throw new ArgumentException("Today is required.", nameof(today));
            
        if (startDate < today)
            throw new ArgumentException("Start date must be today or a future date.", nameof(startDate));

        Id = Guid.NewGuid();
        OrganizationId = organizationId;
        CreatedByUserId = createdByUserId;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        InternalNotes = internalNotes;
        PublicShortDescription = publicShortDescription;
        PublicDetailedDescription = publicDetailedDescription;

        PublicModality = publicModality;
        LocationDetails = locationDetails;
        OnlineParticipationInfo = onlineParticipationInfo;
        Capacity = capacity;
        PublicScheduleText = publicScheduleText;
        PublicInstructorName = publicInstructorName;


        PublicEnrollmentDeadline = publicEnrollmentDeadline;

        PriceAmount = priceAmount;
        PriceCurrency = priceCurrency;
        PriceNote = priceNote;

        IsArchived = false;
        IsPublic = false;
        PublicSlug = null;
        PublicPublishedAtUtc = null;
        Status = ProgramStatus.Active;
        CompletedAtUtc = null;
        CreatedAtUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
    }

    public void Complete(DateTime utcNow)
    {

            throw new InvalidOperationException("Program is already completed.");

        if (Status == ProgramStatus.Archived)
            throw new InvalidOperationException("An archived program cannot be completed.");

        Status = ProgramStatus.Completed;
        UpdatedAtUtc = utcNow;
    }

    public void Archive(DateTime utcNow)
    {
        if (Status == ProgramStatus.Archived)
            throw new InvalidOperationException("Program is already archived.");

        Status = ProgramStatus.Archived;
        IsArchived = true;
        ArchivedAtUtc = utcNow;
        UpdatedAtUtc = utcNow;
    }

    public void Unarchive(DateTime utcNow)
    {
        // ...existing code...
    }

    public void Publish(string slug, DateTime utcNow)
    {
        if (IsPublic)
            throw new InvalidOperationException("Program is already public.");

        IsPublic = true;
        PublicSlug = slug;
        PublicPublishedAtUtc ??= utcNow; // keep existing date if already set
        UpdatedAtUtc = utcNow;
    }

    public void Unpublish(DateTime utcNow)
    {
        if (!IsPublic)
            throw new InvalidOperationException("Program is not public.");

        IsPublic = false;
        UpdatedAtUtc = utcNow;
    }

    public void Update(
        string name,
        DateOnly startDate,
        DateOnly endDate,
        string publicShortDescription,
        ProgramModality publicModality,
        string publicScheduleText,
        DateTime utcNow,
        string? internalNotes = null,
        string? publicDetailedDescription = null,
        string? locationDetails = null,
        string? onlineParticipationInfo = null,
        int? capacity = null,
        string? publicInstructorName = null,
        DateOnly? publicEnrollmentDeadline = null,
        int? priceAmount = null,
        PriceCurrency? priceCurrency = null,
        string? priceNote = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        if (endDate < startDate)
            throw new ArgumentException("End date must be equal to or after start date.", nameof(endDate));

        if (string.IsNullOrWhiteSpace(publicShortDescription))
            throw new ArgumentException("Public short description cannot be null or empty.", nameof(publicShortDescription));

        if (string.IsNullOrWhiteSpace(publicScheduleText))
            throw new ArgumentException("Public schedule text cannot be null or empty.", nameof(publicScheduleText));

        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        PublicShortDescription = publicShortDescription;
        PublicModality = publicModality;
        PublicScheduleText = publicScheduleText;
        InternalNotes = internalNotes;
        PublicDetailedDescription = publicDetailedDescription;
        LocationDetails = locationDetails;
        OnlineParticipationInfo = onlineParticipationInfo;
        Capacity = capacity;
        PublicInstructorName = publicInstructorName;
        PublicEnrollmentDeadline = publicEnrollmentDeadline;
        PriceAmount = priceAmount;
        PriceCurrency = priceAmount is not null ? priceCurrency : null;
        PriceNote = priceAmount is not null ? priceNote : null;
        UpdatedAtUtc = utcNow;
    }
}