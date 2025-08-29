using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Responses;

public class GetStudentsResponse
{
    [Required] public int StudentId { get; set; }
    [Required] public int YearLevelId { get; set; }
    [Required] public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    [Required] public string LastName { get; set; }
    [Phone] public string? Phone { get; set; }
    [EmailAddress] public string? Email { get; set; }
    
    public List<int> ParentIds { get; set; }
    
}