using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class CreateCourseRequest
{
    [Required] public string CourseName { get; set; }
    [Required] [StringLength(10)] public string CourseCode { get; set; }
    public string? CourseDescription { get; set; }
}