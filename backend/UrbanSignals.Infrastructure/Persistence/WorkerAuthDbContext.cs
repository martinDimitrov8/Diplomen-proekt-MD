
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrbanSignals.Infrastructure.Identity;

namespace UrbanSignals.Infrastructure.Persistence;

public class WorkerAuthDbContext
    : IdentityDbContext<WorkerUser, IdentityRole<Guid>, Guid>
{
    public WorkerAuthDbContext(
        DbContextOptions<WorkerAuthDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<WorkerUser>()
            .Property(worker => worker.FullName)
            .HasMaxLength(120)
            .IsRequired();

        builder.Entity<WorkerUser>()
            .HasIndex(worker => worker.NormalizedEmail)
            .IsUnique()
            .HasFilter("[NormalizedEmail] IS NOT NULL");

        SeedRoles(builder);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        var workerRoleId = Guid.Parse(
            "30000000-0000-0000-0000-000000000001");

        builder.Entity<IdentityRole<Guid>>().HasData(
            new IdentityRole<Guid>
            {
                Id = workerRoleId,
                Name = "Worker",
                NormalizedName = "WORKER",
                ConcurrencyStamp = workerRoleId.ToString()
            });
    }
}
