using apbd_tut_9.DTOs;

namespace apbd_tut_9.Services;

public interface IStudentsService
{
    Task<StudentDashboardDto?> GetStudentDashboardAsync(int id);
}