using System.Security.Claims;
using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.GeneralObjects;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.Exceptions;
using Infrastructure.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers.Users;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/user/faculty")]
public class FacultyController : ControllerBase
{
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGetFacultyUseCase _getFacultyUseCase;
    
    public FacultyController(IHttpContextAccessor httpContextAccessor, IGetFacultyUseCase getFacultyUseCase)
    {
        _httpContextAccessor = httpContextAccessor;
        _getFacultyUseCase = getFacultyUseCase;
    }

    [HasPermission("Faculty:create")]
    [HttpPost("invite-faculty")]
    public async Task<IActionResult> InviteFaculty(InviteFacultyRequest request)
    {
        throw new NotImplementedException();
        
        return Ok(new
        {
            Status = true,
            Message = "Faculty created and invited.",
            Data = (object)null,
            Errors = (string[])null
        });
    }

    [HasPermission("Faculty:view")]
    [HttpGet("get-faculty")]
    public async Task<IActionResult> GetFaculty()
    {
        var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

        var facultyList = new List<GetFacultyResponse>();

        try
        {
            var faculty = await _getFacultyUseCase.GetFacultyAsync(schoolId);
            
            foreach (var facultyMember in faculty)
            {
                facultyList.Add(new GetFacultyResponse
                {
                    FacultyId = facultyMember.FacultyId,
                    FirstName = facultyMember.FirstName,
                    LastName = facultyMember.LastName,
                    PhoneNumber = facultyMember.PhoneNumber,
                    Email = facultyMember.Email,
                    DateCreated = facultyMember.DateCreated,
                    DateUpdated = facultyMember.DateUpdated
                });
            }
            
            return Ok( new
                {
                    Status = true,
                    Message = "Retrieved faculty successfully.",
                    Data = facultyList,
                    Errors = (string[])null
                }
            );
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

        return Ok();
    }



}
