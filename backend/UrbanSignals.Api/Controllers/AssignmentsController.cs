using Microsoft.AspNetCore.Mvc;
namespace UrbanSignals.Api.Controllers;
// Етап 1: описателна крайна точка. Назначаването ще бъде реализирано след ролевата авторизация.
[ApiController]
[Route("api/assignments")]
public class AssignmentsController : ControllerBase
{
    [HttpGet("workflow")]
    public IActionResult GetWorkflowStates() => Ok(new[] { "Pending", "Accepted", "InProgress", "Completed", "Cancelled" });
}
