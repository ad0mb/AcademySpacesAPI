using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.GeneralObjects;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using AcademySpacesAPI.WebApi.DTOs.Responses;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/school/administration/classrooms")]
public class ClassroomController : ControllerBase
{
    
    private readonly ICreateClassroomUseCase _createClassroomUseCase;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGetClassroomsUseCase _getClassroomsUseCase;
    private readonly IGetClassroomScheduleUseCase _getClassroomScheduleUseCase;
    private readonly IUpdateClassroomScheduleUseCase _updateClassroomScheduleUseCase;
    private readonly IUpdateClassroomRosterUseCase _updateClassroomRosterUseCase;
    private readonly IGetClassroomRosterUseCase _getClassroomRosterUseCase;
    private readonly IDeleteClassroomUseCase _deleteClassroomUseCase;
    
    public ClassroomController(ICreateClassroomUseCase createClassroomUseCase, IHttpContextAccessor httpContextAccessor, IGetClassroomsUseCase getClassroomsUseCase, IGetClassroomScheduleUseCase getClassroomScheduleUseCase, IUpdateClassroomScheduleUseCase updateClassroomScheduleUseCase, IUpdateClassroomRosterUseCase updateClassroomRosterUseCase, IGetClassroomRosterUseCase getClassroomRosterUseCase, IDeleteClassroomUseCase deleteClassroomUseCase)
    {
        _createClassroomUseCase = createClassroomUseCase;
        _httpContextAccessor = httpContextAccessor;
        _getClassroomsUseCase = getClassroomsUseCase;
        _getClassroomScheduleUseCase = getClassroomScheduleUseCase;
        _updateClassroomScheduleUseCase = updateClassroomScheduleUseCase;
        _updateClassroomRosterUseCase = updateClassroomRosterUseCase;
        _getClassroomRosterUseCase = getClassroomRosterUseCase;
        _deleteClassroomUseCase = deleteClassroomUseCase;
    }
    
    //TODO: Has school wide setting enabled attribute to add
    [HasPermission("Classroom:create")]
    [HttpPost("create-classroom")]
    public async Task<IActionResult> CreateClassroom(CreateClassroomRequest request)
    {
        try
        {
            var cycleId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("cycle_id").Value);
            
            var classroomEntry = new ClassroomEntry
            {
                CycleId = cycleId,
                ClassroomTeacherId = request.ClassroomTeacherId,
                ClassroomName = request.ClassroomName,
            };

            await _createClassroomUseCase.CreateClassroomAsync(classroomEntry);
            
            return Ok(new
            {
                Status = true,
                Message = "Classroom created successfully.",
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
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
        catch (NoRowsAffectedException ex)
        {
            return StatusCode(500, new
            {
                Status = false,
                Message = "Classroom not created.",
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
    }

    [HasPermission("Classroom:view")]
    [HttpGet("get-classrooms")]
    public async Task<IActionResult> GetClassrooms([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? searchTerm)
    {
        try
        {
            var classroomsList = new List<GetClassroomsResponse>();
            
            var schooldId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);
            var cycleId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("cycle_id").Value);

            List<ClassroomEntry> classrooms;
            int count = 0;

            var (sortedList, totalCount) = await _getClassroomsUseCase.GetClassroomsAsync(schooldId, cycleId, pageSize, pageNumber, searchTerm);
            classrooms = sortedList;
            count = totalCount; //TODO: Come back and make sure count is only grabbed in repostiory using a query if pagination is being used, currently it grabs count using a second query on any case.

            foreach (var classroom in classrooms)
            {
                classroomsList.Add(new GetClassroomsResponse
                {
                    ClassroomId = classroom.ClassroomId,
                    ClassroomTeacherId = classroom.ClassroomTeacherId,
                    ClassroomName = classroom.ClassroomName,
                    NumberOfStudents = classroom.NumberOfStudents,
                    ClassroomTeacherName = classroom.ClassroomTeacherName,
                });
            }
            
            return Ok( new
                {
                    Status = true,
                    Message = "Retrieved classrooms successfully.",
                    Data = new
                    {
                        ClassroomsList = classroomsList,
                        TotalCount = count
                    },
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
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
    }
    
    // TODO: Decide on a permission or way to gatekeep retrieving classrooms
    [HttpGet("get-classroom")]
    public async Task<IActionResult> GetClassroom([FromQuery] int classroomId)
    {
        try
        {
            var schooldId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

            throw new NotImplementedException();
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

    [HttpGet("get-classroom-schedule")]
    public async Task<IActionResult> GetClassroomSchedule([FromQuery] int classroomId)
    {

        try
        {
            var periodsList = new List<GetPeriodsResponse>();
            
            var cycleId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("cycle_id").Value);

            var periods = await _getClassroomScheduleUseCase.GetClassroomScheduleAsync(classroomId, cycleId);

            foreach (var period in periods)
            {
                periodsList.Add(new GetPeriodsResponse
                {
                    PeriodId = period.PeriodId,
                    Teacher = period.Teacher == null
                        ? null
                        : new GetFacultyResponse
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
                    PeriodSchedule = period.PeriodSchedule
                        .Select(ps => new GetPeriodScheduleEntryResponse
                        {
                            PsId = ps.Id,
                            DayOfWeek = ps.DayOfWeek,
                            StartTime = ps.StartTime,
                            EndTime = ps.EndTime
                        }).ToList()
                    
                });
            }

            return Ok(new
            {
                Status = true,
                Message = "Retrieved classroom schedule successfully.",
                Data = periodsList,
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

    [HttpPatch("update-classroom-schedule")]
    public async Task<IActionResult> UpdateClassroomSchedule(UpdateClassroomScheduleRequest request)
    {
        try
        {
            var cycleId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("cycle_id").Value);

            await _updateClassroomScheduleUseCase.UpdateClassroomScheduleAsync(cycleId, request.ClassroomId,
                request.PeriodIds);
            
            return Ok(new
            {
                Status = true,
                Message = "Classroom updated successfully.",
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

    [HttpGet("{classroomId}/get-classroom-roster")]
    public async Task<IActionResult> GetClassroomRoster([FromRoute] int classroomId)
    {
        try
        {
            var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

            var cycleId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("cycle_id").Value);

            var students = await _getClassroomRosterUseCase.GetClassroomRosterAsync(schoolId, cycleId, classroomId);
            
            var studentsList = new List<GetStudentsResponse>();

            foreach (var student in students)
            {
                studentsList.Add(new GetStudentsResponse
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
                Message = "Retrieved classroom roster successfully.",
                Data = studentsList,
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
    
    [HttpPatch("{classroomId}/update-classroom-roster")]
    public async Task<IActionResult> UpdateClassroomRoster(UpdateClassroomRosterRequest request, int classroomId)
    {
        try
        {
            var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);
            var cycleId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("cycle_id").Value);

            await _updateClassroomRosterUseCase.UpdateClassroomRoster(schoolId, cycleId, classroomId,
                request.StudentIds);

            return Ok(new
            {
                Status = true,
                Message = "Classroom roster updated successfully.",
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
                Data = (object)null,
                Errors = new[] { ex.Message }
            });
        }
        catch (RosterConflictException ex)
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

    [HttpDelete("delete-classroom/{classroomId}")]
    public async Task<IActionResult> DeleteClassroom(int classroomId)
    {
        try
        {
            var cycleId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("cycle_id").Value);

            await _deleteClassroomUseCase.DeleteClassroomAsync(cycleId, classroomId);
            
            return Ok(new
            {
                Status = true,
                Message = "Classroom deleted successfully.",
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
                Data = (object)null,
                Errors = new[] { ex.Message }
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
    }
}