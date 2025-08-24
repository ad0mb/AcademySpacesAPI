using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class UpdatePeriodScheduleEntryRequest
{
    [Required] public int PsId { get; set; }
    [Required] [Range(1, 7)] public int DayOfWeek { get; set; }
    [Required] public TimeOnly StartTime { get; set; }
    [Required] public TimeOnly EndTime { get; set; }
    [Required] public bool toDelete { get; set; } = false;
}