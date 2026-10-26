using System.ComponentModel.DataAnnotations;
namespace UrbanSignals.Domain.Entities;
public class ReportStatusHistory
{
    public Guid Id { get; set; }
    public Guid ReportId { get; set; }
    public Report Report { get; set; } = null!;
    public ReportStatus? OldStatus { get; set; }
    public ReportStatus NewStatus { get; set; }
    public Guid ChangedByUserId { get; set; }
    public ApplicationUser ChangedByUser { get; set; } = null!;
    public DateTime ChangedAtUtc { get; set; } = DateTime.UtcNow;
    [MaxLength(500)] public string? Note { get; set; }
}
