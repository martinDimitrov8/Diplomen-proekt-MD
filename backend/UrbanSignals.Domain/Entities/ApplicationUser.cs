using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace UrbanSignals.Domain.Entities;
public class ApplicationUser : IdentityUser<Guid>
{
    [Required, MaxLength(120)] public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public ICollection<Report> ReportsCreated { get; set; } = new List<Report>();
    public ICollection<ReportComment> Comments { get; set; } = new List<ReportComment>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Assignment> AssignmentsReceived { get; set; } = new List<Assignment>();
    public ICollection<Assignment> AssignmentsCreated { get; set; } = new List<Assignment>();
    public ICollection<ReportStatusHistory> StatusChanges { get; set; } = new List<ReportStatusHistory>();
    public ICollection<ReportImage> ImagesUploaded { get; set; } = new List<ReportImage>();
}
