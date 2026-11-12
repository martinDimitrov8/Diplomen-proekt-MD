
namespace UrbanSignals.Domain.Entities;

public class Assignment
{
    public Guid Id { get; set; }

    public Guid ReportId { get; set; }
    public Report Report { get; set; } = null!;

    // Worker account from the separate WorkerAuth database.
    public Guid AssignedToWorkerId { get; set; }

    // Main application user who assigned the task.
    public Guid AssignedByUserId { get; set; }

    public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime DueAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }

    public AssignmentStatus Status { get; set; } = AssignmentStatus.Pending;
}
