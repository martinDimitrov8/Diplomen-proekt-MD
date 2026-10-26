using System.ComponentModel.DataAnnotations;
namespace UrbanSignals.Domain.Entities;
public class ReportCategory
{
    public Guid Id { get; set; }
    [Required, MaxLength(80)] public string Name { get; set; } = string.Empty;
    [MaxLength(300)] public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Report> Reports { get; set; } = new List<Report>();
}
