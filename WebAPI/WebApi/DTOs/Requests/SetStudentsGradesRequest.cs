using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class SetStudentsGradesRequest
{
    [Required] public int StudentId { get; set; }
    [Required] public int AssignmentId { get; set; }
    [Required] public int Score { get; set; }
}