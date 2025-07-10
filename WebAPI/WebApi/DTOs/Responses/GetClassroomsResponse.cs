using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Responses;

public class GetClassroomsResponse
{
    [Required] public int ClassroomId { get; set; }
    public int? ClassroomTeacherId { get; set; }
    [Required] public string ClassroomName { get; set; }
    public string ClassroomTeacherName { get; set; }
    [Required] public int NumberOfStudents { get; set; }
}