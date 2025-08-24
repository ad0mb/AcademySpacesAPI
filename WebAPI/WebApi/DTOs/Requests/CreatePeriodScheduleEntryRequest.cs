using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class CreatePeriodScheduleEntryRequest
{
    [Required] [Range(1, 7)] public int DayOfWeek { get; set; }
    [Required] public TimeOnly StartTime { get; set; }
    [Required] public TimeOnly EndTime { get; set; }
}