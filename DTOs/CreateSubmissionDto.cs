using System.ComponentModel.DataAnnotations;

namespace apbd_tut_9.DTOs;

public class CreateSubmissionDto
{
    [Required]
    public int AssignmentId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    [RegularExpression(@"^https://.*", ErrorMessage = "Repository URL must start with https://")]
    public string RepositoryUrl { get; set; } = null!;
}