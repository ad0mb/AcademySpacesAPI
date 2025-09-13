using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class UpdateClassroomScheduleRequest
{
    [Required] public int ClassroomId { get; set; }
    [Required] public List<int> PeriodIds { get; set; }
}