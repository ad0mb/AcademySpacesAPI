using System.Data.Common;
using System.Globalization;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using AcademySpacesAPI.WebApi.DTOs.Responses;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.UseCases;
using Infrastructure.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/school/periods/{periodId}/assignments")]
public class AssignmentController : ControllerBase
{
    
    private readonly PeriodAccessChecker _periodAccessChecker;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGetAssignmentsUseCase _getAssignmentsUseCase;
    private readonly IGetStudentGradesUseCase _getStudentGradesUseCase;
    private readonly ISetStudentsGradesUseCase _setStudentsGradesUseCase;
    
    public AssignmentController(PeriodAccessChecker periodAccessChecker, IHttpContextAccessor httpContextAccessor, IGetAssignmentsUseCase getAssignmentsUseCase, IGetStudentGradesUseCase getStudentGradesUseCase, ISetStudentsGradesUseCase setStudentsGradesUseCase)
    {
        _periodAccessChecker = periodAccessChecker;
        _httpContextAccessor = httpContextAccessor;
        _getAssignmentsUseCase = getAssignmentsUseCase;
        _getStudentGradesUseCase = getStudentGradesUseCase;
        _setStudentsGradesUseCase = setStudentsGradesUseCase;
    }

    [HttpGet("get-assignments")]
    public async Task<IActionResult> GetAssignments(int periodId)
    {
        try
        {
            var result =
                await _periodAccessChecker.CheckAccess(_httpContextAccessor.HttpContext.User, periodId, "view");
            
            if (!result)
            {
                return StatusCode(403, new
                {
                    Status = false,
                    Message = "You are not allowed to access this resource.",
                    Data = (object)null,
                    Errors = (string[])null
                });
            }
            
            var cycleId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("cycle_id").Value);

            var returnList = new List<GetAssignmentsResponse>();

            var assignments = await _getAssignmentsUseCase.GetAssignmentsByPeriodIdAsync(cycleId, periodId);
            
            foreach (var assignment in assignments)
            {
                returnList.Add(new GetAssignmentsResponse
                {
                    AssignmentId = assignment.AssignmentId,
                    AssignmentType = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(assignment.AssignmentType.ToString()).Replace("_", " "),
                    AssignmentName = assignment.AssignmentName,
                    MaxScore = assignment.MaxScore,
                    Description = assignment.Description,
                    DueDate = assignment.DueDate
                });
            }
            
            return Ok(new
            {
                Status = true,
                Message = "Assignments retrieved successfully.",
                Data = returnList,
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
    }

    [HttpGet("get-grades")]
    public async Task<IActionResult> GetGrades(int periodId, [FromQuery] int assignmentId)
    {
        try
        {
            var result =
                await _periodAccessChecker.CheckAccess(_httpContextAccessor.HttpContext.User, periodId, "view");
            
            if (!result)
            {
                return StatusCode(403, new
                {
                    Status = false,
                    Message = "You are not allowed to access this resource.",
                    Data = (object)null,
                    Errors = (string[])null
                });
            }
            
            var cycleId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("cycle_id").Value);
            
            var returnList = new List<GetStudentGradesResponse>();
            
            var grades = await _getStudentGradesUseCase.GetStudentsGradesAsync(cycleId, assignmentId, periodId);
            
            foreach (var grade in grades)
            {
                returnList.Add(new GetStudentGradesResponse
                {
                    AssignmentId = grade.AssignmentId,
                    StudentId = grade.StudentId,
                    Score = grade.Score
                });
            }
            
            return Ok(new
            {
                Status = true,
                Message = "Student grades retrieved successfully.",
                Data = returnList,
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
    }
    
    [HttpPost("set-grades")]
    public async Task<IActionResult> SetGrades(int periodId, List<SetStudentsGradesRequest> request)
    {
        try
        {
            var result =
                await _periodAccessChecker.CheckAccess(_httpContextAccessor.HttpContext.User, periodId, "edit");
            
            if (!result)
            {
                return StatusCode(403, new
                {
                    Status = false,
                    Message = "You are not allowed to access this resource.",
                    Data = (object)null,
                    Errors = (string[])null
                });
            }
            
            var cycleId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("cycle_id").Value);

            var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

            var grades = new List<StudentGradeEntry>();
            
            foreach (var grade in request)
            {
                grades.Add(new StudentGradeEntry
                {
                    AssignmentId = grade.AssignmentId,
                    StudentId = grade.StudentId,
                    Score = grade.Score
                });
            }

            await _setStudentsGradesUseCase.SetStudentsGradesAsync(schoolId, cycleId, periodId, grades);
            
            return Ok(new
            {
                Status = true,
                Message = "Student grades set successfully.",
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
    }
}