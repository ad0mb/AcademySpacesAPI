using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class CreateClassroomRequest
{
    public int? ClassroomTeacherId { get; set; }
    [Required] public string ClassroomName { get; set; }
}