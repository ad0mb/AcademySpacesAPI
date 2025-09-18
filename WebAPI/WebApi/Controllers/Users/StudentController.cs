using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using AcademySpacesAPI.WebApi.DTOs.Responses;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AcademySpacesAPI.WebApi.Controllers.Users;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/user/student")]
public class StudentController : ControllerBase
{
    
    private readonly IGetStudentsUseCase _getStudentsUseCase;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICreateStudentUseCase _createStudentUseCase;
    private readonly IUpdateStudentUseCase _updateStudentUseCase;
    
    public StudentController(IGetStudentsUseCase getStudentsUseCase, IHttpContextAccessor httpContextAccessor, ICreateStudentUseCase createStudentUseCase, IUpdateStudentUseCase updateStudentUseCase)
    {
        _getStudentsUseCase = getStudentsUseCase;
        _httpContextAccessor = httpContextAccessor;
        _createStudentUseCase = createStudentUseCase;
        _updateStudentUseCase = updateStudentUseCase;
    }

    //TODO: CHECK STUDENT YEAR LEVELS FOREIGN KEY ON DELETE ANED ON UPDATE CASCADE OPTIONS
    [EnableRateLimiting("fixed")] 
    [HasPermission("Student:view")]
    [HttpGet("get-students")]
    public async Task<IActionResult> GetStudents([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? searchTerm, [FromQuery] int yearLevelId, [FromQuery] int classroomId, [FromQuery] int periodId, [FromQuery] bool noClassroom = false)
    {

        var returnList = new List<GetStudentsResponse>();
        
        try
        {
            var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);
            
            var cycleId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("cycle_id").Value);

            var (students, totalCount) = await _getStudentsUseCase.GetStudentsAsync(schoolId, cycleId, pageSize, pageNumber, searchTerm, yearLevelId, classroomId, periodId, noClassroom);

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
    [EnableRateLimiting("fixed")] 
    [HasPermission("Student:create")]
    [HttpPost("create-student")]
    public async Task<IActionResult> CreateStudent(CreateStudentRequest request)
    {
        try
        {
            var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

            var student = new StudentEntry
            {
                SchoolId = schoolId,
                YearLevelId = request.YearLevelId,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Phone = request.Phone,
                Email = request.Email,
                ParentIds = request.ParentIds ?? new HashSet<int>()
            };

            await _createStudentUseCase.CreateStudentAsync(student);
            
            return Ok(new
            {
                Status = true,
                Message = "Student created successfully.",
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
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
    }
    [EnableRateLimiting("fixed")] 
    [HasPermission("Student:update")]
    [HttpPatch("update-student")]
    public async Task<IActionResult> UpdateStudent(UpdateStudentRequest request)
    {
        try
        {
            var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

            var student = new StudentEntry
            {
                StudentId = request.StudentId,
                SchoolId = schoolId,
                YearLevelId = request.YearLevelId,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Phone = request.Phone,
                Email = request.Email,
                ParentIds = request.ParentIds ?? new HashSet<int>()
            };

            await _updateStudentUseCase.UpdateStudentAsync(student);
            
            return Ok(new
            {
                Status = true,
                Message = "Student updated successfully.",
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
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new
            {
                Status = false,
                Message = "Student to update was not found.",
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
    }
}