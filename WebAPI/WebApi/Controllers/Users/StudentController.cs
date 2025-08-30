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

    //TODO: CHECK STUDENT YEAR LEVELS FOREIGN KEY ON DELETE ANED ON UPDATE CASCADE OPTIONS
    
    [HasPermission("Student:view")]
    [HttpGet("get-students")]
    public async Task<IActionResult> GetStudents([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? searchTerm, [FromQuery] int yearLevelId)
    {

        var returnList = new List<GetStudentsResponse>();
        
        try
        {
            var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

            var (students, totalCount) = await _getStudentsUseCase.GetStudentsAsync(schoolId, pageSize, pageNumber, searchTerm, yearLevelId);

            foreach (var student in students)
            {
                returnList.Add(new GetStudentsResponse
                {
                    
                    StudentId = student.StudentId,
                    YearLevelId = student.YearLevelId,
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
                Data = new
                {
                    StudentsList = returnList,
                    TotalCount = totalCount
                },
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