using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbanSignals.Infrastructure.Persistence;
namespace UrbanSignals.Api.Controllers;
[ApiController]
[Route("api/reports/{reportId:guid}/comments")]
public class CommentsController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPublicComments(Guid reportId, CancellationToken cancellationToken)
    {
        var exists = await db.Reports.AnyAsync(x => x.Id == reportId, cancellationToken);
        if (!exists) return NotFound();
        var comments = await db.ReportComments.AsNoTracking().Where(x => x.ReportId == reportId && x.IsPublic)
            .OrderBy(x => x.CreatedAtUtc).Select(x => new { x.Id, x.Body, x.CreatedAtUtc }).ToListAsync(cancellationToken);
        return Ok(comments);
    }
}
