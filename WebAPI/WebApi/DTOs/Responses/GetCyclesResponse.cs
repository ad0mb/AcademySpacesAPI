using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Responses;

public class GetCyclesResponse
{
    [Required] public int CycleId { get; set; }
    [Required] public bool IsActive { get; set; }
    [Required] public bool isArchived { get; set; }
    [Required] public string CycleName { get; set; }
    [Required] public string Code { get; set; }
    [Required] public int ScheduleType { get; set; }
    [Required] public DateOnly StartDate { get; set; }
    [Required] public DateOnly EndDate { get; set; }

    [Required] public List<GetGradingPeriodsResponse> GradingPeriods { get; set; }
    [Required]public bool IsExpired { get; set; }
}