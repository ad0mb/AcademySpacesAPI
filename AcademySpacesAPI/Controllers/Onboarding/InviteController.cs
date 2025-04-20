using AcademySpacesAPI.Data.Auth;
using AcademySpacesAPI.Exceptions;
using AcademySpacesAPI.Models.DatabaseModels;
using AcademySpacesAPI.Models.DatabaseModels.Users;
using AcademySpacesAPI.Models.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace AcademySpacesAPI.Controllers.Onboarding;

[ApiController]
[Route("api/onboarding/invite")]
public class InviteController : ControllerBase
{
    
    private readonly SchoolRepo _schoolRepo;
    private readonly UserRepo _userRepo;
    private readonly RoleRepo _roleRepo;

    public InviteController(SchoolRepo schoolRepo, UserRepo userRepo, RoleRepo roleRepo)
    {
        _schoolRepo = schoolRepo;
        _userRepo = userRepo;
        _roleRepo = roleRepo;
    }

    //TODO: Implement email 6 digit code verification feature (after endpoint request is sent or before)
    //TODO: Try catch, test how exceptions are handled, test overal function and all subsidaries
    //TODO: Add exception to a future logger to prevent leaking internal information about api or db
    [Authorize(AuthenticationSchemes =
        "SchoolRegistrationBearer")] //checks bearer assigned from school registration link
    [HttpPost("register-school")]
    public async Task<IActionResult> RegisterSchoolAndAdmin(RegisterRequest request)
    {
        try
        {
            var ids = await _schoolRepo.CreateSchoolAsync(new Schools
            {
                Name = request.SchoolName,
                CountryOfOrigin = request.SchoolCountry
            });
            
            await _userRepo.CreateFacultyAsync(new Faculty
            {
                SchoolId = ids[0],
                IdentityId = request.IdentityId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.SigninEmail,
            }, [ids[1]]);

            return Ok(new
            {
                Status = true,
                Message = "School and Admin succesfully registered.",
                Data = (object[])null,
                Errors = (string[])null
            });
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new
            {
                Status = false,
                Message = "Database error occured.",
                Data = (object[])null,
                Errors = new[] { "Wrong input data or database/query error." }
            });
        }
        catch (NoRowsAffectedException ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new
            {
                Status = false,
                Message = "No rows affected.",
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, new
            {
                Status = false,
                Message = "An unplanned error occured.",
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }
    }
}