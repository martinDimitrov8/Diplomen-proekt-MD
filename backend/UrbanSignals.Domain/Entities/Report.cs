using System.ComponentModel.DataAnnotations;
namespace UrbanSignals.Domain.Entities;
public class Report
{
    public Guid Id { get; set; }
    [Required, MaxLength(120)] public string Title { get; set; } = string.Empty;
    [Required, MinLength(20), MaxLength(3000)] public string Description { get; set; } = string.Empty;
    [Required, MaxLength(200)] public string Address { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public ReportStatus Status { get; set; } = ReportStatus.Submitted;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public Guid CategoryId { get; set; }
    public ReportCategory Category { get; set; } = null!;
    public Guid CreatedByUserId { get; set; }
    public ApplicationUser CreatedByUser { get; set; } = null!;
    public ICollection<ReportImage> Images { get; set; } = new List<ReportImage>();
    public ICollection<ReportStatusHistory> StatusHistory { get; set; } = new List<ReportStatusHistory>();
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public ICollection<ReportComment> Comments { get; set; } = new List<ReportComment>();
}
