using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.Responses;
using Core.ApplicationCore.Interfaces.UseCases;
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
    
    public CoursesController(IHttpContextAccessor httpContextAccessor, IGetCoursesUseCase getCoursesUseCase)
    {
        _httpContextAccessor = httpContextAccessor;
        _getCoursesUseCase = getCoursesUseCase;
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
        catch (Exception ex)
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