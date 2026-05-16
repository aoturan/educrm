using EduCrm.Infrastructure.Persistence;
using EduCrm.Infrastructure.RateLimiting;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.People.Domain.Entities;
using EduCrm.Modules.Program.Domain.Entities;
using EduCrm.Modules.Support.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduCrm.Infrastructure.Data;


public sealed class AppDbContext : DbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // DbSet'ler public olmak zorunda değil; ama pratik olsun diye public bırakıyoruz.
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<User> Users => Set<User>();
    public DbSet<EduCrm.Modules.Account.Domain.Entities.Subscription> Subscriptions => Set<EduCrm.Modules.Account.Domain.Entities.Subscription>();
    public DbSet<SubscriptionRequest> SubscriptionRequests => Set<SubscriptionRequest>();
    public DbSet<SubscriptionHistory> SubscriptionHistories => Set<SubscriptionHistory>();
    public DbSet<SubscriptionPaymentNotification> SubscriptionPaymentNotifications => Set<SubscriptionPaymentNotification>();
    public DbSet<OrganizationBillingDetail> OrganizationBillingDetails => Set<OrganizationBillingDetail>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<FollowUp> FollowUps => Set<FollowUp>();

    public DbSet<Program> Programs => Set<Program>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<SupportRequest> SupportRequests => Set<SupportRequest>();
    public DbSet<SupportContactMessage> SupportContactMessages => Set<SupportContactMessage>();

    public DbSet<RateLimitCounter> RateLimitCounters => Set<RateLimitCounter>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}