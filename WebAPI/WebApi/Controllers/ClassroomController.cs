using AcademySpacesAPI.WebApi.Attributes;
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
    
    public ClassroomController(ICreateClassroomUseCase createClassroomUseCase, IHttpContextAccessor httpContextAccessor, IGetClassroomsUseCase getClassroomsUseCase)
    {
        _createClassroomUseCase = createClassroomUseCase;
        _httpContextAccessor = httpContextAccessor;
        _getClassroomsUseCase = getClassroomsUseCase;
    }
    
    //TODO: Has school wide setting enabled attribute to add
    [HasPermission("Classroom:create")]
    [HttpPost("create-classroom")]
    public async Task<IActionResult> CreateClassroom(CreateClassroomRequest request)
    {
        try
        {
            var schooldId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);
            
            var classroomEntry = new ClassroomEntry
            {
                SchoolId = schooldId,
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

            List<ClassroomEntry> classrooms;
            int count = 0;

            var (sortedList, totalCount) = await _getClassroomsUseCase.GetClassroomsAsync(schooldId, pageSize, pageNumber, searchTerm);
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
    
}