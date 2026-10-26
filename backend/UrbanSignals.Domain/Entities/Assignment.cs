namespace UrbanSignals.Domain.Entities;
public class Assignment
{
    public Guid Id { get; set; }
    public Guid ReportId { get; set; }
    public Report Report { get; set; } = null!;
    public Guid AssignedToUserId { get; set; }
    public ApplicationUser AssignedToUser { get; set; } = null!;
    public Guid AssignedByUserId { get; set; }
    public ApplicationUser AssignedByUser { get; set; } = null!;
    public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime DueAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public AssignmentStatus Status { get; set; } = AssignmentStatus.Pending;
}
