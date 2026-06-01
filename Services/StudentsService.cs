using apbd_tut_9.Data;
using apbd_tut_9.DTOs;
using Microsoft.EntityFrameworkCore;

namespace apbd_tut_9.Services;

public class StudentsService : IStudentsService
{
    private readonly UniversityTasksDbContext _context;
    
    public StudentsService(UniversityTasksDbContext context)
    {
        _context = context;
    }
    
    public async Task<StudentDashboardDto?> GetStudentDashboardAsync(int id)
    {
        var query = await _context.Students.AsNoTracking()
            .Where(s => s.StudentId == id)
            .Select(s => new StudentDashboardDto
            {
                StudentId = s.StudentId,
                IndexNumber = s.IndexNumber,
                FullName = $"{s.FirstName} {s.LastName}",
                IsActive = s.IsActive,
                Enrollments = s.Enrollments
                    .Select(e => e.Course.Name)
                    .ToList(),
                Submissions = s.Submissions
                    .Select(sub => new SubmissionDto
                    {
                        SubmissionId = sub.SubmissionId,
                        StudentName = $"{s.FirstName} {s.LastName}",
                        AssignmentTitle = sub.Assignment.Title,
                        RepositoryUrl = sub.RepositoryUrl,
                        Status = sub.Status,
                        Score = sub.Score,
                        Feedback = sub.Feedback
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
        return query;
    }
}