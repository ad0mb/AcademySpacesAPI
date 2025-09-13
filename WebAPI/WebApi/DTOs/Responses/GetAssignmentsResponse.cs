using System.ComponentModel.DataAnnotations;
using Core.ApplicationCore.Enums;

namespace AcademySpacesAPI.WebApi.DTOs.Responses;

public class GetAssignmentsResponse
{
    [Required] public int AssignmentId { get; set; }
    [Required] public string AssignmentType { get; set; }
    [Required] public string AssignmentName { get; set; }
    [Required] public int MaxScore { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
}