using System.ComponentModel.DataAnnotations;
using AcademySpacesAPI.WebApi.DTOs.GeneralObjects;

namespace AcademySpacesAPI.WebApi.DTOs.Responses;

public class GetPeriodsResponse
{
    [Required] public int PeriodId { get; set; }
    public GetFacultyResponse? Teacher { get; set; }
    [Required] public GetCoursesResponse Course { get; set; }
    [Required] public string Name { get; set; }
    public string? Location { get; set; }
    [Required] public List<GetPeriodScheduleEntryResponse> PeriodSchedule { get; set; }
}