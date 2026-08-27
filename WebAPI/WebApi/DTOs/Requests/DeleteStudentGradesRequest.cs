using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class DeleteStudentGradesRequest
{
    [Required] public int AssignmentId { get; set; }
    [Required] public List<int> StudentIds { get; set; } = new();
}
