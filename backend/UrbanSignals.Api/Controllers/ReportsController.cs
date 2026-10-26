using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbanSignals.Domain.Entities;
using UrbanSignals.Infrastructure.Persistence;
namespace UrbanSignals.Api.Controllers;
[ApiController]
[Route("api/reports")]
public class ReportsController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? status, [FromQuery] Guid? categoryId, CancellationToken cancellationToken)
    {
        var query = db.Reports.AsNoTracking().Include(x => x.Category).AsQueryable();
        if (categoryId.HasValue) query = query.Where(x => x.CategoryId == categoryId.Value);
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!Enum.TryParse<ReportStatus>(status, true, out var parsedStatus)) return BadRequest(new { message = "Невалиден статус." });
            query = query.Where(x => x.Status == parsedStatus);
        }
        var reports = await query.OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new { x.Id, x.Title, x.Address, x.Status, x.CreatedAtUtc, Category = x.Category.Name }).ToListAsync(cancellationToken);
        return Ok(reports);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var report = await db.Reports.AsNoTracking().Include(x => x.Category).Where(x => x.Id == id)
            .Select(x => new { x.Id, x.Title, x.Description, x.Address, x.Latitude, x.Longitude, x.Status, x.CreatedAtUtc, Category = x.Category.Name })
            .SingleOrDefaultAsync(cancellationToken);
        return report is null ? NotFound() : Ok(report);
    }
}
