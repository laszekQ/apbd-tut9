using apbd_tut_9.DTOs;
using apbd_tut_9.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_tut_9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubmissionsController : ControllerBase
{
    private readonly ISubmissionsService _submissionsService;

    public SubmissionsController(ISubmissionsService submissionsService)
    {
        _submissionsService = submissionsService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateSubmission([FromBody] CreateSubmissionDto dto)
    {
        try
        {
            var result = await _submissionsService.CreateSubmissionAsync(dto);
            return CreatedAtAction(nameof(CreateSubmission), new { id = result.SubmissionId }, result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}/grade")]
    public async Task<IActionResult> GradeSubmission(int id, [FromBody] GradeSubmissionDto dto)
    {
        try
        {
            await _submissionsService.GradeSubmissionAsync(id, dto);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            if (ex.Message.Contains("not found"))
                return NotFound(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSubmission(int id)
    {
        try
        {
            await _submissionsService.DeleteSubmissionAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}