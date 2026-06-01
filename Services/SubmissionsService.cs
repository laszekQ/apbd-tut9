using apbd_tut_9.Data;
using apbd_tut_9.DTOs;
using apbd_tut_9.Models;
using Microsoft.EntityFrameworkCore;

namespace apbd_tut_9.Services;

public class SubmissionsService : ISubmissionsService
{
    private readonly UniversityTasksDbContext _context;

    public SubmissionsService(UniversityTasksDbContext context)
    {
        _context = context;
    }

    public async Task<SubmissionDto> CreateSubmissionAsync(CreateSubmissionDto dto)
    {
        var student = await _context.Students
            .Include(s => s.Enrollments)
            .FirstOrDefaultAsync(s => s.StudentId == dto.StudentId);
        if (student == null)
            throw new ArgumentException("No such student");
        if (!student.IsActive)
            throw new InvalidOperationException("The student is inactive");

        var assignment = await _context.Assignments
            .FirstOrDefaultAsync(a => a.AssignmentId == dto.AssignmentId);
        if (assignment == null || !assignment.IsPublished)
            throw new ArgumentException("The assignment either exists not or is unpublished.");

        var isEnrolled = student.Enrollments.Any(e => 
            e.CourseId == assignment.CourseId && 
            (e.Status.Equals("Active", StringComparison.OrdinalIgnoreCase) || 
             e.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase)));
        if (!isEnrolled)
            throw new InvalidOperationException("The student is not enrolled in the course that owns this assignment");

        var alreadySubmitted = await _context.Submissions
            .AnyAsync(s => s.StudentId == dto.StudentId && s.AssignmentId == dto.AssignmentId);
        if (alreadySubmitted)
            throw new InvalidOperationException("The student hath already submitted this assignment");

        string status = DateTime.UtcNow > assignment.DueDate ? "Late" : "Submitted";

        var submission = new Submission
        {
            AssignmentId = dto.AssignmentId,
            StudentId = dto.StudentId,
            RepositoryUrl = dto.RepositoryUrl,
            SubmittedAt = DateTime.UtcNow,
            Status = status
        };

        _context.Submissions.Add(submission);
        await _context.SaveChangesAsync();

        return new SubmissionDto
        {
            SubmissionId = submission.SubmissionId,
            StudentName = $"{student.FirstName} {student.LastName}",
            AssignmentTitle = assignment.Title,
            RepositoryUrl = submission.RepositoryUrl,
            Status = submission.Status,
            Score = submission.Score,
            Feedback = submission.Feedback
        };
    }

    public async Task GradeSubmissionAsync(int id, GradeSubmissionDto dto)
    {
        var submission = await _context.Submissions
            .Include(s => s.Assignment)
            .FirstOrDefaultAsync(s => s.SubmissionId == id);

        if (submission == null)
            throw new ArgumentException("No such submission");
        if (dto.Score < 0 || dto.Score > submission.Assignment.MaxPoints)
            throw new ArgumentException($"The score must be between 0 and the assignment max points");

        submission.Score = dto.Score;
        submission.Feedback = dto.Feedback;
        submission.Status = "Graded";

        await _context.SaveChangesAsync();
    }

    public async Task DeleteSubmissionAsync(int id)
    {
        var submission = await _context.Submissions
            .FirstOrDefaultAsync(s => s.SubmissionId == id);

        if (submission == null)
            throw new ArgumentException("No such submission");
        if (submission.Status.Equals("Graded", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("A graded submission can't be deleted");

        _context.Submissions.Remove(submission);
        await _context.SaveChangesAsync();
    }
}