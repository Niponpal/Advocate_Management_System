using AMS.Auth_IdentityModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

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

    public DbSet<Advocate> Advocates { get; set; }
    public DbSet<AdvocateSchedule> AdvocateSchedules { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Case> Cases { get; set; }
    public DbSet<CaseCategory> CaseCategories { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Court> Courts { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Hearing> Hearings { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<LegalNotice> LegalNotices { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<TaskManagement> TaskManagements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations from the assembly

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
