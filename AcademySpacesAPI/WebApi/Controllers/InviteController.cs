using AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;
using AcademySpacesAPI.Exceptions;
using AcademySpacesAPI.WebApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace AcademySpacesAPI.WebApi.Controllers;

[ApiController]
[Route("api/onboarding/invite")]
public class InviteController : ControllerBase
{
    
    private readonly IRegisterSchoolAndAdminUseCase _registerSchoolAndAdminUseCase;

    public InviteController(IRegisterSchoolAndAdminUseCase registerSchoolAndAdminUseCase)
    {
        _registerSchoolAndAdminUseCase = registerSchoolAndAdminUseCase;
    }

    //TODO: Implement email 6 digit code verification feature (after endpoint request is sent or before)
    //TODO: Try catch, test how exceptions are handled, test overal function and all subsidaries
    //TODO: Add exception to a future logger to prevent leaking internal information about api or db
    [Authorize(AuthenticationSchemes =
        "SchoolRegistrationBearer")] //checks bearer assigned from school registration link
    [HttpPost("register-school")]
    public async Task<IActionResult> RegisterSchoolAndAdmin(RegisterSchoolRequest request)
    {
        await _registerSchoolAndAdminUseCase.CreateSchoolAndAdminAsync(request);

        return Ok(new
        {
            Status = true,
            Message = "School and Admin succesfully registered.",
            Data = (object[])null,
            Errors = (string[])null
        });
    }
}