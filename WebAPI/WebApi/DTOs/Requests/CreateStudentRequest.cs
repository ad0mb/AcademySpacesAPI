using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class CreateStudentRequest
{
    [Required] public int YearLevelId { get; set; }
    [Required] public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    [Required] public string LastName { get; set; }
    [Phone] public string? Phone { get; set; }
    [EmailAddress] public string? Email { get; set; }
    public HashSet<int>? ParentIds { get; set; }
}