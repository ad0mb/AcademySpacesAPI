using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class CreatePeriodRequest
{
    public int? TeacherId { get; set; }
    [Required] public int CourseId { get; set; }
    [Required] public string Name { get; set; }
    public string? Location { get; set; }
    [Range(1, 7)] public int? DayOfWeek { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
}