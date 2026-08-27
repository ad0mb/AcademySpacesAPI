using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class UpdateStudentGradesRequest
{
    [Required] public int AssignmentId { get; set; }
    [Required] public List<StudentGradeRequestItem> Grades { get; set; } = new();
}
