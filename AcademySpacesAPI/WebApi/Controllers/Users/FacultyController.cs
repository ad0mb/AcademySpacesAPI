using System.Security.Claims;
using AcademySpacesAPI.Data;
using AcademySpacesAPI.Infrastructure.Firebase;
using AcademySpacesAPI.Infrastructure.Persistence.Models;
using AcademySpacesAPI.Webapi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.Webapi.Controllers.Users;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/user/faculty")]
public class FacultyController : ControllerBase
{

    private readonly FirebaseAuthService _firebaseAuthService;
    private readonly UserRepo _userRepo;

    public FacultyController(FirebaseAuthService firebaseAuthService, UserRepo userRepo)
    {
        _firebaseAuthService = firebaseAuthService;
        _userRepo = userRepo;
    }

    [HasPermission("Faculty:create")]
    [HttpPost("invite-faculty")]
    public async Task<IActionResult> InviteFaculty(Faculty request)
    {
        var identityId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(identityId))
        {
            return BadRequest(new
            {
                Status = false,
                Message = "Identity ID is null or empty.",
                Data = (object)null,
                Errors = new[] { "Identity ID is null or empty." }
            });
        }

        var
            faculty = await _userRepo
                .GetFacultyAsync(
                    identityId); //no need to check if user making request is null or verify permissions, HasPermission attribute verifies permissions, and multiple layers prevent user from not being linked to an account.

        if (faculty == null)
        {
            return BadRequest(new
            {
                Status = false,
                Message = "Faculty member not found.",
                Data = (object)null,
                Errors = new[] { "Faculty not found." }
            });
        }
        
        await _userRepo.CreateFacultyAsync(new Faculty
        {
            SchoolId = faculty.SchoolId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email
        }, []);
        
        //add email and token invitation logic
        
        return Ok(new
        {
            Status = true,
            Message = "Faculty created and invited.",
            Data = (object)null,
            Errors = (string[])null
        });
    }



}
