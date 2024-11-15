using Microsoft.AspNetCore.Mvc;

namespace Lil.TimeTracker.Controllers;

[Route("")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<Resources.Resource>>(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        var resources = new List<Resources.Resource>{
            new Resources.Resource("Employees", "/api/Employee"),
            new Resources.Resource("Projects", "/api/Project"),
            new Resources.Resource("Time Entries", "/api/TimeEntry"),
            new Resources.Resource("Project Assignments", "/api/ProjectAssignments")
        };

        return Ok(resources);
    }
}