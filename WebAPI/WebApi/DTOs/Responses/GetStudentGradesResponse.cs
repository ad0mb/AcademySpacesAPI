using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Responses;

public class GetStudentGradesResponse
{
    [Required] public int AssignmentId { get; set; }
    [Required] public int StudentId { get; set; }
    [Required] public int Score { get; set; }
}