using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using AcademySpacesAPI.WebApi.DTOs.Responses;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers.SchoolSettings;

[ApiController]
[Route("api/school/settings/courses")]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
public class CoursesController : ControllerBase
{
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGetCoursesUseCase _getCoursesUseCase;
    private readonly ICreateCourseUseCase _createCourseUseCase;
    
    public CoursesController(IHttpContextAccessor httpContextAccessor, IGetCoursesUseCase getCoursesUseCase, ICreateCourseUseCase createCourseUseCase)
    {
        _httpContextAccessor = httpContextAccessor;
        _getCoursesUseCase = getCoursesUseCase;
        _createCourseUseCase = createCourseUseCase;
    }
    
    [HasPermission("Courses:view")]
    [HttpGet("get-courses")]
    public async Task<IActionResult> GetCourses()
    {
        try
        {
            var courses = new List<GetCoursesResponse>();

            var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

            var courseEntries = await _getCoursesUseCase.GetCoursesAsync(schoolId);

            foreach (var course in courseEntries)
            {
                courses.Add(new GetCoursesResponse
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    CourseCode = course.CourseCode,
                    CourseDescription = course.CourseDescription,
                });
            }
            
            return Ok(new
            {
                Status = true,
                Message = "Retrieved courses successfully.",
                Data = courses,
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

    [HasPermission("Courses:create")]
    [HttpPost("create-course")]
    public async Task<IActionResult> CreateCourse(CreateCourseRequest createCourseRequest)
    {
        try
        {
            var schooldId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

            var courseEntry = new CourseEntry
            {
                SchoolId = schooldId,
                CourseName = createCourseRequest.CourseName,
                CourseCode = createCourseRequest.CourseCode,
                CourseDescription = createCourseRequest.CourseDescription,
            };

            await _createCourseUseCase.CreateCourseAsync(courseEntry);

            return Ok(new
            {
                Status = true,
                Message = "Course created successfully.",
                Data = new { },
                Errors = (string[])null
            });
        }
        catch (DuplicateNameException ex)
        {
            return StatusCode(409, new
            {
                Status = false,
                Message = ex.Message,
                Data = (object)null,
                Errors = new[] { ex.Message }
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
    
}