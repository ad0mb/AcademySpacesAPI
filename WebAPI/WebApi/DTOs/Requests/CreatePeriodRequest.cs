using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class CreatePeriodRequest
{
    public int? TeacherId { get; set; }
    [Required] public int CourseId { get; set; }
    [Required] public string Name { get; set; }
    public string? Location { get; set; }
    [Required] public List<CreatePeriodScheduleEntryRequest> PeriodSchedule { get; set; }
}