using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.GeneralObjects;

public class GetFacultyResponse
{
    [Required] public int FacultyId { get; set; }
    [Required] public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    [Required] public string LastName { get; set; }
    [Phone] public string PhoneNumber { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}