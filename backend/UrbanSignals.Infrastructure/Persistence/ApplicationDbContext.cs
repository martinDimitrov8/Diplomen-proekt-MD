
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrbanSignals.Domain.Entities;
using UrbanSignals.Infrastructure.Identity;

namespace UrbanSignals.Infrastructure.Persistence;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

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

        ConfigureUsers(builder);
        ConfigureReports(builder);
        ConfigureReportImages(builder);
        ConfigureReportHistory(builder);
        ConfigureAssignments(builder);
        ConfigureComments(builder);
        ConfigureNotifications(builder);

        SeedRoles(builder);
        SeedCategories(builder);
    }

    private static void ConfigureUsers(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>()
            .Property(user => user.FullName)
            .HasMaxLength(120)
            .IsRequired();
    }

    private static void ConfigureReports(ModelBuilder builder)
    {
        builder.Entity<Report>()
            .Property(report => report.Title)
            .HasMaxLength(120)
            .IsRequired();

        builder.Entity<Report>()
            .Property(report => report.Description)
            .HasMaxLength(3000)
            .IsRequired();

        builder.Entity<Report>()
            .Property(report => report.Address)
            .HasMaxLength(200)
            .IsRequired();

        builder.Entity<Report>()
            .Property(report => report.Latitude)
            .HasPrecision(9, 6);

        builder.Entity<Report>()
            .Property(report => report.Longitude)
            .HasPrecision(9, 6);

        builder.Entity<Report>()
            .HasIndex(report => new
            {
                report.Status,
                report.CreatedAtUtc
            });

        builder.Entity<Report>().ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_Reports_Latitude_Range",
                "[Latitude] IS NULL OR ([Latitude] >= -90 AND [Latitude] <= 90)");

            table.HasCheckConstraint(
                "CK_Reports_Longitude_Range",
                "[Longitude] IS NULL OR ([Longitude] >= -180 AND [Longitude] <= 180)");
        });

        builder.Entity<Report>()
            .HasOne(report => report.Category)
            .WithMany(category => category.Reports)
            .HasForeignKey(report => report.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Report>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(report => report.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureReportImages(ModelBuilder builder)
    {
        builder.Entity<ReportImage>()
            .Property(image => image.ImageUrl)
            .HasMaxLength(500)
            .IsRequired();

        builder.Entity<ReportImage>()
            .HasOne(image => image.Report)
            .WithMany(report => report.Images)
            .HasForeignKey(image => image.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureReportHistory(ModelBuilder builder)
    {
        builder.Entity<ReportStatusHistory>()
            .Property(history => history.Note)
            .HasMaxLength(500);

        builder.Entity<ReportStatusHistory>()
            .HasOne(history => history.Report)
            .WithMany(report => report.StatusHistory)
            .HasForeignKey(history => history.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureAssignments(ModelBuilder builder)
    {
        builder.Entity<Assignment>()
            .HasOne(assignment => assignment.Report)
            .WithMany(report => report.Assignments)
            .HasForeignKey(assignment => assignment.ReportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Assignment>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(assignment => assignment.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureComments(ModelBuilder builder)
    {
        builder.Entity<ReportComment>()
            .Property(comment => comment.Body)
            .HasMaxLength(1500)
            .IsRequired();

        builder.Entity<ReportComment>()
            .HasOne(comment => comment.Report)
            .WithMany(report => report.Comments)
            .HasForeignKey(comment => comment.ReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureNotifications(ModelBuilder builder)
    {
        builder.Entity<Notification>()
            .Property(notification => notification.Title)
            .HasMaxLength(120)
            .IsRequired();

        builder.Entity<Notification>()
            .Property(notification => notification.Message)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Entity<Notification>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(notification => notification.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        var roles = new[]
        {
            "Citizen",
            "Operator",
            "Administrator"
        };

        var roleIds = new[]
        {
            Guid.Parse("20000000-0000-0000-0000-000000000001"),
            Guid.Parse("20000000-0000-0000-0000-000000000002"),
            Guid.Parse("20000000-0000-0000-0000-000000000004")
        };

        for (var index = 0; index < roles.Length; index++)
        {
            builder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid>
                {
                    Id = roleIds[index],
                    Name = roles[index],
                    NormalizedName = roles[index].ToUpperInvariant(),
                    ConcurrencyStamp = roleIds[index].ToString()
                });
        }
    }

    private static void SeedCategories(ModelBuilder builder)
    {
        builder.Entity<ReportCategory>().HasIndex(category => category.Name)
            .IsUnique();

        builder.Entity<ReportCategory>().HasData(
            new ReportCategory
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                Name = "Пътища и тротоари",
                Description = "Дупки, повредени настилки и тротоари",
                IsActive = true
            },
            new ReportCategory
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                Name = "Чистота",
                Description = "Отпадъци и нерегламентирани сметища",
                IsActive = true
            },
            new ReportCategory
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                Name = "Улично осветление",
                Description = "Неработещи или повредени осветителни тела",
                IsActive = true
            },
            new ReportCategory
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000004"),
                Name = "Паркове и зелени площи",
                Description = "Проблеми в паркове, градини и площадки",
                IsActive = true
            },
            new ReportCategory
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000005"),
                Name = "Други",
                Description = "Други проблеми в градската среда",
                IsActive = true
            });
    }
}
