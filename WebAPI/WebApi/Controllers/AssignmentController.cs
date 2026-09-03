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
    private readonly ICreateAssignmentUseCase _createAssignmentUseCase;
    private readonly IUpdateAssignmentUseCase _updateAssignmentUseCase;
    private readonly IDeleteAssignmentUseCase _deleteAssignmentUseCase;
    private readonly ICreateStudentGradesUseCase _createStudentGradesUseCase;
    private readonly IUpdateStudentGradesUseCase _updateStudentGradesUseCase;
    private readonly IDeleteStudentGradesUseCase _deleteStudentGradesUseCase;
    
    public AssignmentController(PeriodAccessChecker periodAccessChecker, IHttpContextAccessor httpContextAccessor, IGetAssignmentsUseCase getAssignmentsUseCase, IGetStudentGradesUseCase getStudentGradesUseCase, ICreateAssignmentUseCase createAssignmentUseCase, IUpdateAssignmentUseCase updateAssignmentUseCase, IDeleteAssignmentUseCase deleteAssignmentUseCase, ICreateStudentGradesUseCase createStudentGradesUseCase, IUpdateStudentGradesUseCase updateStudentGradesUseCase, IDeleteStudentGradesUseCase deleteStudentGradesUseCase)
    {
        _periodAccessChecker = periodAccessChecker;
        _httpContextAccessor = httpContextAccessor;
        _getAssignmentsUseCase = getAssignmentsUseCase;
        _getStudentGradesUseCase = getStudentGradesUseCase;
        _createAssignmentUseCase = createAssignmentUseCase;
        _updateAssignmentUseCase = updateAssignmentUseCase;
        _deleteAssignmentUseCase = deleteAssignmentUseCase;
        _createStudentGradesUseCase = createStudentGradesUseCase;
        _updateStudentGradesUseCase = updateStudentGradesUseCase;
        _deleteStudentGradesUseCase = deleteStudentGradesUseCase;
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

    [HttpPost("create-assignment")]
    public async Task<IActionResult> CreateAssignment(int periodId, CreateAssignmentRequest request)
    {
        try
        {
            var result =
                await _periodAccessChecker.CheckAccess(_httpContextAccessor.HttpContext.User, periodId, "create");

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

            var assignmentEntry = new AssignmentEntry
            {
                PeriodId = periodId,
                AssignmentType = request.AssignmentType,
                AssignmentName = request.AssignmentName,
                MaxScore = request.MaxScore,
                Description = request.Description,
                DueDate = request.DueDate
            };

            await _createAssignmentUseCase.CreateAssignmentAsync(assignmentEntry);

            return Ok(new
            {
                Status = true,
                Message = "Assignment created successfully.",
                Data = new { },
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
        catch (Core.Exceptions.NoRowsAffectedException ex)
        {
            return StatusCode(500, new
            {
                Status = false,
                Message = "Assignment not created.",
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
    }

    [HttpPatch("update-assignment")]
    public async Task<IActionResult> UpdateAssignment(int periodId, UpdateAssignmentRequest request)
    {
        try
        {
            var result =
                await _periodAccessChecker.CheckAccess(_httpContextAccessor.HttpContext.User, periodId, "update");

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

            var assignmentEntry = new AssignmentEntry
            {
                AssignmentId = request.AssignmentId,
                PeriodId = periodId,
                AssignmentType = request.AssignmentType,
                AssignmentName = request.AssignmentName,
                MaxScore = request.MaxScore,
                Description = request.Description,
                DueDate = request.DueDate
            };

            await _updateAssignmentUseCase.UpdateAssignmentAsync(assignmentEntry);

            return Ok(new
            {
                Status = true,
                Message = "Assignment updated successfully.",
                Data = new { },
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
        catch (Core.Exceptions.NotFoundException ex)
        {
            return NotFound(new
            {
                Status = false,
                Message = ex.Message,
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }
    }

    [HttpDelete("delete-assignment")]
    public async Task<IActionResult> DeleteAssignment(int periodId, [FromQuery] int assignmentId)
    {
        try
        {
            var result =
                await _periodAccessChecker.CheckAccess(_httpContextAccessor.HttpContext.User, periodId, "delete");

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

            await _deleteAssignmentUseCase.DeleteAssignmentAsync(assignmentId, periodId);

            return Ok(new
            {
                Status = true,
                Message = "Assignment deleted successfully.",
                Data = new { },
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
        catch (Core.Exceptions.NotFoundException ex)
        {
            return NotFound(new
            {
                Status = false,
                Message = ex.Message,
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }
    }

    [HttpPost("create-grades")]
    public async Task<IActionResult> CreateGrades(int periodId, CreateStudentGradesRequest request)
    {
        try
        {
            var result =
                await _periodAccessChecker.CheckAccess(_httpContextAccessor.HttpContext.User, periodId, "create");

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

            var gradeEntries = new List<StudentGradeEntry>();

            foreach (var grade in request.Grades)
            {
                gradeEntries.Add(new StudentGradeEntry
                {
                    AssignmentId = request.AssignmentId,
                    StudentId = grade.StudentId,
                    Score = grade.Score
                });
            }

            await _createStudentGradesUseCase.CreateStudentGradesAsync(request.AssignmentId, periodId, gradeEntries);

            return Ok(new
            {
                Status = true,
                Message = "Student grades created successfully.",
                Data = new { },
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
        catch (Core.Exceptions.NoRowsAffectedException ex)
        {
            return StatusCode(500, new
            {
                Status = false,
                Message = "Grades not created.",
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
        catch (Core.Exceptions.NotFoundException ex)
        {
            return NotFound(new
            {
                Status = false,
                Message = ex.Message,
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }
        catch (Core.Exceptions.InvalidScoreException ex)
        {
            return BadRequest(new
            {
                Status = false,
                Message = ex.Message,
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
        catch (Core.Exceptions.RosterConflictException ex)
        {
            return StatusCode(409, new
            {
                Status = false,
                Message = ex.Message,
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
    }

    [HttpPatch("update-grades")]
    public async Task<IActionResult> UpdateGrades(int periodId, UpdateStudentGradesRequest request)
    {
        try
        {
            var result =
                await _periodAccessChecker.CheckAccess(_httpContextAccessor.HttpContext.User, periodId, "update");

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

            var gradeEntries = new List<StudentGradeEntry>();

            foreach (var grade in request.Grades)
            {
                gradeEntries.Add(new StudentGradeEntry
                {
                    AssignmentId = request.AssignmentId,
                    StudentId = grade.StudentId,
                    Score = grade.Score
                });
            }

            await _updateStudentGradesUseCase.UpdateStudentGradesAsync(request.AssignmentId, periodId, gradeEntries);

            return Ok(new
            {
                Status = true,
                Message = "Student grades updated successfully.",
                Data = new { },
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
        catch (Core.Exceptions.NotFoundException ex)
        {
            return NotFound(new
            {
                Status = false,
                Message = ex.Message,
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }
        catch (Core.Exceptions.InvalidScoreException ex)
        {
            return BadRequest(new
            {
                Status = false,
                Message = ex.Message,
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
        catch (Core.Exceptions.RosterConflictException ex)
        {
            return StatusCode(409, new
            {
                Status = false,
                Message = ex.Message,
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
    }

    [HttpDelete("delete-grades")]
    public async Task<IActionResult> DeleteGrades(int periodId, DeleteStudentGradesRequest request)
    {
        try
        {
            var result =
                await _periodAccessChecker.CheckAccess(_httpContextAccessor.HttpContext.User, periodId, "delete");

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

            await _deleteStudentGradesUseCase.DeleteStudentGradesAsync(request.AssignmentId, periodId, request.StudentIds);

            return Ok(new
            {
                Status = true,
                Message = "Student grades deleted successfully.",
                Data = new { },
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
        catch (Core.Exceptions.NotFoundException ex)
        {
            return NotFound(new
            {
                Status = false,
                Message = ex.Message,
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }
        catch (Core.Exceptions.RosterConflictException ex)
        {
            return StatusCode(409, new
            {
                Status = false,
                Message = ex.Message,
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
    }
}