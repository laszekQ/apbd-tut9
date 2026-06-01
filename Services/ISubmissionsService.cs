using apbd_tut_9.DTOs;

namespace apbd_tut_9.Services;

public interface ISubmissionsService
{
    Task<SubmissionDto> CreateSubmissionAsync(CreateSubmissionDto dto);
    Task GradeSubmissionAsync(int id, GradeSubmissionDto dto);
    Task DeleteSubmissionAsync(int id);
}