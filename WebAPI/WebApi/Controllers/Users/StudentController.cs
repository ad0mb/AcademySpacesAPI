using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using AcademySpacesAPI.WebApi.DTOs.Responses;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers.Users;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/user/student")]
public class StudentController : ControllerBase
{
    
    private readonly IGetStudentsUseCase _getStudentsUseCase;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public StudentController(IGetStudentsUseCase getStudentsUseCase, IHttpContextAccessor httpContextAccessor)
    {
        _getStudentsUseCase = getStudentsUseCase;
        _httpContextAccessor = httpContextAccessor;
    }

    [HasPermission("Student:view")]
    [HttpGet("get-students")]
    public async Task<IActionResult> CreateStudent()
    {

        var returnList = new List<GetStudentsResponse>();
        
        try
        {
            var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

            var students = await _getStudentsUseCase.GetStudentsAsync(schoolId);

            foreach (var student in students)
            {
                returnList.Add(new GetStudentsResponse
                {
                    StudentId = student.StudentId,
                    FirstName = student.FirstName,
                    MiddleName = student.MiddleName,
                    LastName = student.LastName,
                    Phone = student.Phone,
                    Email = student.Email,
                    ParentIds = student.ParentIds
                });
            }

            return Ok(new
            {
                Status = true,
                Message = "Students retrieved successfully.",
                Data = returnList,
                Errors = (string[])null
            });
            
        } catch (DbException ex)
        {
            return StatusCode(500, new
            {
                Status = false,
                Message = "Internal server error.",
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
    }
}