using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Responses;

//TODO: Add snaitation (such as string length) to all properties from all responses and requests
public class GetCoursesResponse
{
    [Required] public int CourseId { get; set; }
    [Required] public string CourseName { get; set; }
    [Required] [StringLength(10)] public string CourseCode { get; set; }
    public string? CourseDescription { get; set; }
}