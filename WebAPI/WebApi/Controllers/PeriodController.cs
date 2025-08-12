using AcademySpacesAPI.WebApi.DTOs.GeneralObjects;
using AcademySpacesAPI.WebApi.DTOs.Responses;
using Core.ApplicationCore.Interfaces.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademySpacesAPI.WebApi.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/school/periods")]
public class PeriodController : ControllerBase
{

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGetPeriodsUseCase _getPeriodsUseCase;

    public PeriodController(IHttpContextAccessor httpContextAccessor, IGetPeriodsUseCase getPeriodsUseCase)
    {
        _httpContextAccessor = httpContextAccessor;
        _getPeriodsUseCase = getPeriodsUseCase;
    }

    [HttpGet("get-periods")]
    public async Task<IActionResult> GetPeriods([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? searchTerm, [FromQuery] int facultyId, [FromQuery] int courseId, [FromQuery] TimeOnly? startTime, [FromQuery] TimeOnly? endTime)
    {
        try
        {
            var periods = new List<GetPeriodsResponse>();

            var schooldId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

            var (periodsList, totalCount) = await _getPeriodsUseCase.GetPeriodsBySchoolAsync(schooldId, pageSize, pageNumber, searchTerm, facultyId, courseId, startTime, endTime);

            foreach (var period in periodsList)
            {
                periods.Add(new GetPeriodsResponse
                {
                    PeriodId = period.PeriodId,
                    Teacher = period.Teacher == null ? null : new GetFacultyResponse
                    {
                        FacultyId = period.Teacher.FacultyId,
                        FirstName = period.Teacher.FirstName,
                        MiddleName = period.Teacher.MiddleName,
                        LastName = period.Teacher.LastName,
                        PhoneNumber = period.Teacher.PhoneNumber,
                        Email = period.Teacher.Email,
                    },
                    Course = new GetCoursesResponse
                    {
                        CourseId = period.Course.CourseId,
                        CourseName = period.Course.CourseName,
                        CourseCode = period.Course.CourseCode,
                        CourseDescription = period.Course.CourseDescription,
                    },
                    Location = period.Location,
                    Capacity = period.Capacity,
                    DayOfWeek = period.DayOfWeek,
                    StartTime = period.StartTime,
                    EndTime = period.EndTime
                });
            }

            return Ok(new
            {
                Status = true,
                Message = "Periods retrieved successfully.",
                Data = new 
                {
                    PeriodsList = periods,
                    TotalCount = totalCount
                },
                Errors = (string[])null
            });
        }
        catch (DbUpdateException ex)
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

    [HttpPost("create-period")]
    public async Task<IActionResult> CreatePeriod()
    {
        throw new NotImplementedException();
    }
}