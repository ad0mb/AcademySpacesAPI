using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class StudentGradeRequestItem
{
    [Required] public int StudentId { get; set; }
    [Required] public int Score { get; set; }
}
