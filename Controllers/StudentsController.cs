using apbd_tut_9.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_tut_9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentsService _studentsService;
    
    public StudentsController(IStudentsService studentsService)
    {
        _studentsService = studentsService;
    }

    [HttpGet]
    [Route("{id}/dashboard")]
    public async Task<IActionResult> GetStudentDashboardAsync(int id)
    {
        var studentDashboard = await _studentsService.GetStudentDashboardAsync(id);
        if(studentDashboard == null)
            return NotFound("No such student");
        return Ok(studentDashboard);
    }
}