using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class UpdatePeriodRequest
{
    [Required] public int PeriodId { get; set; }
    public int? TeacherId { get; set; }
    [Required] public int CourseId { get; set; }
    [Required] public string Name { get; set; }
    public string? Location { get; set; }
    public List<UpdatePeriodScheduleEntryRequest> PeriodSchedule { get; set; }
}