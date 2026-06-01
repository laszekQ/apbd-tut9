namespace apbd_tut_9.DTOs;

public class StudentDashboardDto
{
    public int StudentId { get; set; }
    public string IndexNumber { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public bool IsActive { get; set; }
    public ICollection<string> Enrollments { get; set; } = new List<string>();
    public ICollection<SubmissionDto> Submissions { get; set; } = new List<SubmissionDto>();
}