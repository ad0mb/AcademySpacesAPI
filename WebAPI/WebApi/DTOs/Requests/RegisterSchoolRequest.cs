using System.ComponentModel.DataAnnotations;

namespace AcademySpacesAPI.WebApi.DTOs.Requests;

public class RegisterSchoolRequest
{
    [Required] public string SchoolName { get; set; }
    [Required] public string SchoolCountry { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] [EmailAddress] public string SigninEmail { get; set; }
    [Required] public string IdentityId { get; set; }
}