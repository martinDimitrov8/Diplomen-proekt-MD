using Microsoft.AspNetCore.Mvc;
namespace UrbanSignals.Api.Controllers;
// Само описателна крайна точка за етапа на концепцията. Административните операции изискват авторизация.
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    [HttpGet("planned-roles")]
    public IActionResult GetPlannedRoles() => Ok(new[] { "Citizen", "Operator", "Executor", "Administrator" });
}
