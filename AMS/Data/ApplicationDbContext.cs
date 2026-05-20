using Microsoft.EntityFrameworkCore;

namespace AMS.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<Advocate> advocates { get; set; }
    public DbSet<AdvocateSchedule> advocateSchedules { get; set; }
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

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Client)
            .WithMany(c => c.Payments)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.NoAction);
    }

}
