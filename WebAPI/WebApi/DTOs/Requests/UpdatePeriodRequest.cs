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
    
    public class UpdatePeriodScheduleEntryRequest
    {
        [Required] public int PsId { get; set; }
        [Required] [Range(1, 7)] public int DayOfWeek { get; set; }
        [Required] public TimeOnly StartTime { get; set; }
        [Required] public TimeOnly EndTime { get; set; }
        [Required] public bool toDelete { get; set; } = false;
    }
}