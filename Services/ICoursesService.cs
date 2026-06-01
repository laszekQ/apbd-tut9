using apbd_tut_9.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace apbd_tut_9.Services;

public interface ICoursesService
{
    Task<List<CourseDto>> GetCoursesAsync(bool activeOnly);
    Task<List<AssignmentDto>?> GetAssignmentsAsync(int id, bool publishedOnly);
}