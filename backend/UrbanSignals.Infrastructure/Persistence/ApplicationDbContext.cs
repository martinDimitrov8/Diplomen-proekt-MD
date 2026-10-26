using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrbanSignals.Domain.Entities;

namespace UrbanSignals.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<ReportCategory> ReportCategories => Set<ReportCategory>();
    public DbSet<ReportImage> ReportImages => Set<ReportImage>();
    public DbSet<ReportStatusHistory> ReportStatusHistory => Set<ReportStatusHistory>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<ReportComment> ReportComments => Set<ReportComment>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().Property(x => x.FullName).HasMaxLength(120).IsRequired();
        builder.Entity<ReportCategory>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<ReportCategory>().Property(x => x.Name).HasMaxLength(80).IsRequired();
        builder.Entity<Report>().Property(x => x.Title).HasMaxLength(120).IsRequired();
        builder.Entity<Report>().Property(x => x.Description).HasMaxLength(3000).IsRequired();
        builder.Entity<Report>().Property(x => x.Address).HasMaxLength(200).IsRequired();
        builder.Entity<Report>().Property(x => x.Latitude).HasPrecision(9, 6);
        builder.Entity<Report>().Property(x => x.Longitude).HasPrecision(9, 6);
        builder.Entity<Report>().HasIndex(x => new { x.Status, x.CreatedAtUtc });
        builder.Entity<Report>().ToTable(table =>
        {
            table.HasCheckConstraint("CK_Reports_Latitude_Range", "[Latitude] IS NULL OR ([Latitude] >= -90 AND [Latitude] <= 90)");
            table.HasCheckConstraint("CK_Reports_Longitude_Range", "[Longitude] IS NULL OR ([Longitude] >= -180 AND [Longitude] <= 180)");
        });
        builder.Entity<Report>().HasOne(x => x.Category).WithMany(x => x.Reports).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Report>().HasOne(x => x.CreatedByUser).WithMany(x => x.ReportsCreated).HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<ReportImage>().HasOne(x => x.Report).WithMany(x => x.Images).HasForeignKey(x => x.ReportId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<ReportImage>().HasOne(x => x.UploadedByUser).WithMany(x => x.ImagesUploaded).HasForeignKey(x => x.UploadedByUserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<ReportStatusHistory>().HasOne(x => x.Report).WithMany(x => x.StatusHistory).HasForeignKey(x => x.ReportId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<ReportStatusHistory>().HasOne(x => x.ChangedByUser).WithMany(x => x.StatusChanges).HasForeignKey(x => x.ChangedByUserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Assignment>().HasOne(x => x.Report).WithMany(x => x.Assignments).HasForeignKey(x => x.ReportId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Assignment>().HasOne(x => x.AssignedToUser).WithMany(x => x.AssignmentsReceived).HasForeignKey(x => x.AssignedToUserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Assignment>().HasOne(x => x.AssignedByUser).WithMany(x => x.AssignmentsCreated).HasForeignKey(x => x.AssignedByUserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<ReportComment>().HasOne(x => x.Report).WithMany(x => x.Comments).HasForeignKey(x => x.ReportId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<ReportComment>().HasOne(x => x.Author).WithMany(x => x.Comments).HasForeignKey(x => x.AuthorId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Notification>().HasOne(x => x.User).WithMany(x => x.Notifications).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ReportCategory>().HasData(
            new ReportCategory { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Пътища и тротоари", Description = "Дупки, повредени настилки и тротоари", IsActive = true },
            new ReportCategory { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Name = "Чистота", Description = "Отпадъци и нерегламентирани сметища", IsActive = true },
            new ReportCategory { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Name = "Улично осветление", Description = "Неработещи или повредени осветителни тела", IsActive = true },
            new ReportCategory { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Name = "Паркове и зелени площи", Description = "Проблеми в паркове, градини и площадки", IsActive = true },
            new ReportCategory { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Name = "Други", Description = "Други проблеми в градската среда", IsActive = true }
        );

        var roles = new[] { "Citizen", "Operator", "Executor", "Administrator" };
        var roleIds = new[] {
            Guid.Parse("20000000-0000-0000-0000-000000000001"),
            Guid.Parse("20000000-0000-0000-0000-000000000002"),
            Guid.Parse("20000000-0000-0000-0000-000000000003"),
            Guid.Parse("20000000-0000-0000-0000-000000000004")
        };
        for (var i = 0; i < roles.Length; i++)
            builder.Entity<IdentityRole<Guid>>().HasData(new IdentityRole<Guid> { Id = roleIds[i], Name = roles[i], NormalizedName = roles[i].ToUpperInvariant(), ConcurrencyStamp = roleIds[i].ToString() });
    }
}
