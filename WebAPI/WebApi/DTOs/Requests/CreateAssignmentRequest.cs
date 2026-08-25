using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class CreateAssignmentRequest
{
    [Required] public string AssignmentType { get; set; }
    [Required] [MaxLength(50)] public string AssignmentName { get; set; }
    [Required] public int MaxScore { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
}
