using EduCrm.Infrastructure.Persistence;
using EduCrm.Modules.Account.Domain.Entities;
using EduCrm.Modules.People.Domain.Entities;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}