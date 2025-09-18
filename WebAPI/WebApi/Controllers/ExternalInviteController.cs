using AcademySpacesAPI.WebApi.DTOs.Requests;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MySqlConnector;

namespace AcademySpacesAPI.WebApi.Controllers;

[ApiController]
[Route("api/onboarding/invite")]
public class ExternalInviteController : ControllerBase
{
    
    private readonly IRegisterSchoolAndAdminUseCase _registerSchoolAndAdminUseCase;

    public ExternalInviteController(IRegisterSchoolAndAdminUseCase registerSchoolAndAdminUseCase)
    {
        _registerSchoolAndAdminUseCase = registerSchoolAndAdminUseCase;
    }

    //TODO: Implement email 6 digit code verification feature (after endpoint request is sent or before)
    //TODO: Try catch, test how exceptions are handled, test overal function and all subsidaries
    //TODO: Add exception to a future logger to prevent leaking internal information about api or db
    [EnableRateLimiting("fixed")] 
    [Authorize(AuthenticationSchemes =
        "SchoolRegistrationBearer")] //checks bearer assigned from school registration link
    [HttpPost("register-school")]
    public async Task<IActionResult> RegisterSchoolAndAdmin(RegisterSchoolRequest request)
    {
        try
        {
            await _registerSchoolAndAdminUseCase.CreateSchoolAndAdminAsync(request.SchoolName, request.SchoolCountry, request.FirstName, request.LastName, request.SigninEmail, request.IdentityId);
        }
        catch (DbException ex)
        {
            return StatusCode(500, new
            {
                Status = false,
                Message = "Internal server error.",
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }

        return Ok(new
        {
            Status = true,
            Message = "School and Admin succesfully registered.",
            Data = (object[])null,
            Errors = (string[])null
        });
    }
}