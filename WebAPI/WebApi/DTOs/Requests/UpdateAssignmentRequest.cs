using System.ComponentModel.DataAnnotations;
using Core.ApplicationCore.Enums;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class UpdateAssignmentRequest
{
    [Required] public int AssignmentId { get; set; }
    [Required] public AssignmentType AssignmentType { get; set; }
    [Required] [MaxLength(50)] public string AssignmentName { get; set; }
    [Required] public int MaxScore { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
}
