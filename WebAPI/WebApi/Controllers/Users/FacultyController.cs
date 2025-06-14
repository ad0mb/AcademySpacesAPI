using System.Security.Claims;
using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.GeneralObjects;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using Core.ApplicationCore.DomainEntities;
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
    private readonly IInviteFacultyUseCase _inviteFacultyUseCase;
    private readonly IUpdateFacultyUseCase _updateFacultyUseCase;
    
    public FacultyController(IHttpContextAccessor httpContextAccessor, IGetFacultyUseCase getFacultyUseCase, IInviteFacultyUseCase inviteFacultyUseCase, IUpdateFacultyUseCase updateFacultyUseCase)
    {
        _httpContextAccessor = httpContextAccessor;
        _getFacultyUseCase = getFacultyUseCase;
        _inviteFacultyUseCase = inviteFacultyUseCase;
        _updateFacultyUseCase = updateFacultyUseCase;
    }

    [HasPermission("Faculty:create")]
    [HttpPost("invite-faculty")]
    public async Task<IActionResult> InviteFaculty(InviteFacultyRequest request)
    {
        var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

        try
        {

            await _inviteFacultyUseCase.InviteFacultyAsync(new FacultyEntry
            {
                SchoolId = schoolId,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email
            }, request.Invite);

            return Ok(new
            {
                Status = true,
                Message = "Faculty created" + (request.Invite ? " and invited successfully." : " successfully."),
                Data = (object)null,
                Errors = (string[])null
            });
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
        catch (DuplicateEmailException ex)
        {
            return StatusCode(409, new
            {
                Status = false,
                Message = ex.Message,
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }
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
    }

    [HasPermission("Faculty:update")]
    [HttpPatch("update-faculty")]
    public async Task<IActionResult> UpdateFaculty(UpdateFacultyRequest request)
    {
        try
        {
            await _updateFacultyUseCase.UpdateFacultyAsync(new FacultyEntry
            {
                FacultyId = request.FacultyId,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
            });

            return Ok(new
            {
                Status = true,
                Message = "Faculty updated successfully.",
                Data = (object)null,
                Errors = (string[])null
            });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new
            {
                Status = false,
                Message = ex.Message,
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
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
    }

}
