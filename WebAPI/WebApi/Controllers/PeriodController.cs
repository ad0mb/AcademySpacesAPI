using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.GeneralObjects;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using AcademySpacesAPI.WebApi.DTOs.Responses;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.Exceptions;
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
    private readonly ICreatePeriodUseCase _createPeriodUseCase;

    public PeriodController(IHttpContextAccessor httpContextAccessor, IGetPeriodsUseCase getPeriodsUseCase, ICreatePeriodUseCase createPeriodUseCase)
    {
        _httpContextAccessor = httpContextAccessor;
        _getPeriodsUseCase = getPeriodsUseCase;
        _createPeriodUseCase = createPeriodUseCase;
    }

    [HasPermission("Periods:view")]
    [HttpGet("get-periods")]
    public async Task<IActionResult> GetPeriods([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? searchTerm, [FromQuery] int facultyId, [FromQuery] int courseId, [FromQuery] TimeOnly? startTime, [FromQuery] TimeOnly? endTime)
    {
        try
        {
            var periods = new List<GetPeriodsResponse>();

            var schooldId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);
            
            var cycleId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("cycle_id").Value);

            var (periodsList, totalCount) = await _getPeriodsUseCase.GetPeriodsBySchoolAsync(schooldId, cycleId, pageSize, pageNumber, searchTerm, facultyId, courseId, startTime, endTime);

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
                    Name = period.Name,
                    Location = period.Location,
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

    [HasPermission("Periods:create")]
    [HttpPost("create-period")]
    public async Task<IActionResult> CreatePeriod(CreatePeriodRequest request)
    {
        try
        {
            var cycleId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("cycle_id").Value);

            var periodEntry = new PeriodEntry
            {
                CycleId = cycleId,
                TeacherId = request.TeacherId,
                CourseId = request.CourseId,
                Name = request.Name,
                Location = request.Location,
                DayOfWeek = request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
            };

            await _createPeriodUseCase.CreatePeriodAsync(periodEntry);

            return Ok(new
            {
                Status = true,
                Message = "Period created successfully.",
                Data = new { },
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
        catch (NoRowsAffectedException ex)
        {
            return StatusCode(500, new
            {
                Status = false,
                Message = "Period not created.",
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
        catch (SchedulingConflictException ex)
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