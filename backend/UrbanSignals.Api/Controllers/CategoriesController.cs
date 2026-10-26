using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbanSignals.Infrastructure.Persistence;
namespace UrbanSignals.Api.Controllers;
[ApiController]
[Route("api/categories")]
public class CategoriesController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var categories = await db.ReportCategories.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.Name)
            .Select(x => new { x.Id, x.Name, x.Description }).ToListAsync(cancellationToken);
        return Ok(categories);
    }
}
