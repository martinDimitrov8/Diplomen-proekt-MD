
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace UrbanSignals.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    [Required]
    [MaxLength(120)]
    public string FullName { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
