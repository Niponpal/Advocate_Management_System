using AMS.Auth_IdentityModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace AMS.Data;

public class ApplicationDbContext : IdentityDbContext<
    IdentityModel.User,
    IdentityModel.Role,
    long,
    IdentityModel.UserClaim,
    IdentityModel.UserRole,
    IdentityModel.UserLogin,
    IdentityModel.RoleClaim,
    IdentityModel.UserToken>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Advocate> advocates { get; set; }
    public DbSet<AdvocateSchedule> AdvocateSchedules { get; set; }
    public DbSet<Appointment> appointments { get; set; }
    public DbSet<Case> cases { get; set; }
    public DbSet<CaseCategory> caseCategories { get; set; }
    public DbSet<Client> clients { get; set; }
    public DbSet<Court> courts { get; set; }
    public DbSet<Document> documents { get; set; }
    public DbSet<Hearing> hearings { get; set; }
    public DbSet<Invoice> invoices { get; set; }
    public DbSet<LegalNotice> legalNotices { get; set; }
    public DbSet<Payment> payments { get; set; }
    public DbSet<TaskManagement> taskManagements { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        // Case -> Advocate (One-to-Many)
        modelBuilder.Entity<Case>()
            .HasOne(c => c.Advocate)
            .WithMany(a => a.Cases)
            .HasForeignKey(c => c.AdvocateId)
            .OnDelete(DeleteBehavior.Restrict);

        // Case -> Client (One-to-Many)
        modelBuilder.Entity<Case>()
            .HasOne(c => c.Client)
            .WithMany(cl => cl.Cases)
            .HasForeignKey(c => c.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Appointment -> Advocate
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Advocate)
            .WithMany()
            .HasForeignKey(a => a.AdvocateId)
            .OnDelete(DeleteBehavior.Restrict);

        // Appointment -> Client
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Client)
            .WithMany()
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Hearing -> Case
        modelBuilder.Entity<Hearing>()
            .HasOne(h => h.Case)
            .WithMany(c => c.Hearings)
            .HasForeignKey(h => h.CaseId);

        // Hearing -> Court
        modelBuilder.Entity<Hearing>()
            .HasOne(h => h.Court)
            .WithMany(c => c.Hearings)
            .HasForeignKey(h => h.CourtId);

        // Document -> Case
        modelBuilder.Entity<Document>()
            .HasOne(d => d.Case)
            .WithMany(c => c.Documents)
            .HasForeignKey(d => d.CaseId);

        // Payment -> Case
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Case)
            .WithMany()
            .HasForeignKey(p => p.CaseId);

        // Payment -> Client
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Client)
            .WithMany()
            .HasForeignKey(p => p.ClientId);

        base.OnModelCreating(modelBuilder);
        // Automatically apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // Ignore pending model warnings
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

        // Debug logging
        optionsBuilder.LogTo(Console.WriteLine);
        optionsBuilder.UseLoggerFactory(new LoggerFactory(new[] {
                new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
            }));
    }
}
