using System.ComponentModel.DataAnnotations;
namespace UrbanSignals.Domain.Entities;
public class ReportImage
{
    public Guid Id { get; set; }
    public Guid ReportId { get; set; }
    public Report Report { get; set; } = null!;
    [Required, MaxLength(500)] public string ImageUrl { get; set; } = string.Empty;
    public Guid UploadedByUserId { get; set; }
    public ApplicationUser UploadedByUser { get; set; } = null!;
    public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;
}
