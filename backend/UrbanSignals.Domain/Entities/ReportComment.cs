using System.ComponentModel.DataAnnotations;
namespace UrbanSignals.Domain.Entities;
public class ReportComment
{
    public Guid Id { get; set; }
    public Guid ReportId { get; set; }
    public Report Report { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public ApplicationUser Author { get; set; } = null!;
    [Required, MinLength(1), MaxLength(1500)] public string Body { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public bool IsPublic { get; set; } = true;
}
