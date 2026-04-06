using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.People.Domain.Entities;
using EduCrm.Modules.Program.Domain.Entities;
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
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<FollowUp> FollowUps => Set<FollowUp>();
    
    public DbSet<Program> Programs => Set<Program>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}