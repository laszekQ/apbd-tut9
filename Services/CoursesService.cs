using apbd_tut_9.Data;
using apbd_tut_9.DTOs;
using Microsoft.EntityFrameworkCore;

namespace apbd_tut_9.Services;


public class CoursesService : ICoursesService
{
    private readonly UniversityTasksDbContext _context;
    
    public CoursesService(UniversityTasksDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<CourseDto>> GetCoursesAsync(bool activeOnly)
    {
        var query = _context.Courses.AsNoTracking();

        if (activeOnly)
        {
            query = query.Where(c => c.IsActive);
        }

        return await query
            .Select(c => new CourseDto
            {
                CourseId = c.CourseId,
                Code = c.Code,
                Name = c.Name,
                Credits = c.Credits,
                AssignmentCount = c.Assignments.Count
            })
            .ToListAsync();
    }

    public async Task<List<AssignmentDto>?> GetAssignmentsAsync(int id, bool publishedOnly)
    {
        var query = _context.Assignments.AsNoTracking();
        if (!query.Any(a => a.CourseId == id))
            return null;
        
        if (publishedOnly)
            query = query.Where(a => a.IsPublished);

        return await query
            .Select(a => new AssignmentDto
            {
                AssignmentId = a.AssignmentId,
                Title = a.Title,
                DueDate = a.DueDate,
                MaxPoints = a.MaxPoints,
                IsPublished = a.IsPublished,
                SubmissionCount = a.Submissions.Count
            }).ToListAsync();
    }
}