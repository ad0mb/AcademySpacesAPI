using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class UpdateCourseRequest
{
    [Required] public int CourseId { get; set; }
    [Required] public string CourseName { get; set; }
    [Required] [StringLength(10)] public string CourseCode { get; set; }
    public string? CourseDescription { get; set; }
}