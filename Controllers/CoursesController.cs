using apbd_tut_9.DTOs;
using apbd_tut_9.Models;
using apbd_tut_9.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_tut_9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICoursesService _coursesService;

    public CoursesController(ICoursesService coursesService)
    {
        _coursesService = coursesService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCourses([FromQuery] bool activeOnly = true)
    {
        var courses = await _coursesService.GetCoursesAsync(activeOnly);
        return Ok(courses);
    }

    [HttpGet("{id}/assignments")]
    public async Task<IActionResult> GetAssignments(int id, [FromQuery] bool activeOnly = true)
    {
        List<AssignmentDto>? assignments = await _coursesService.GetAssignmentsAsync(id, activeOnly);
        if(assignments == null)
            return NotFound("No such course");
        
        return Ok(assignments);
    }
}