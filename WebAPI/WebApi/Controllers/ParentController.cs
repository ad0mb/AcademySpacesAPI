using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using AcademySpacesAPI.WebApi.DTOs.Responses;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AcademySpacesAPI.WebApi.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/user/parents")]
public class ParentController : ControllerBase
{
    private readonly ICreateParentUseCase _createParentUseCase;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGetParentsUseCase _getParentsUseCase;
    private readonly IUpdateParentUseCase _updateParentUseCase;
    private readonly IDeleteParentUseCase _deleteParentUseCase;
    
    public ParentController(ICreateParentUseCase createParentUseCase, IHttpContextAccessor httpContextAccessor, IGetParentsUseCase getParentsUseCase, IUpdateParentUseCase updateParentUseCase, IDeleteParentUseCase deleteParentUseCase)
    {
        _createParentUseCase = createParentUseCase;
        _httpContextAccessor = httpContextAccessor;
        _getParentsUseCase = getParentsUseCase;
        _updateParentUseCase = updateParentUseCase;
        _deleteParentUseCase = deleteParentUseCase;
    }
    [EnableRateLimiting("fixed")] 
    [HasPermission("Parent:create")]
    [HttpPost("create-parent")]
    public async Task<IActionResult> CreateParent(CreateParentRequest request)
    {
        var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

        try
        {
            await _createParentUseCase.CreateParentAsync(new ParentEntry
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                Phone = request.PhoneNumber,
                Email = request.Email,
            }, schoolId);

            return Ok(new
            {
                Status = true,
                Message = "Parent created and invited.",
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
        catch (DuplicateEmailException ex)
        {
            return StatusCode(409, new
            {
                Status = false,
                Message = ex.Message,
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }
    }
    [EnableRateLimiting("fixed")] 
    [HasPermission("Parent:view")]
    [HttpGet("get-parents")]
    public async Task<IActionResult> GetParents([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? searchTerm)
    {
        var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

        var parentsList = new List<GetParentsResponse>();

        try
        {
            var (parents, totalCount) = await _getParentsUseCase.GetParentsAsync(schoolId, pageSize, pageNumber, searchTerm);

            foreach (var parent in parents)
            {
                parentsList.Add(new GetParentsResponse
                {
                    ParentId = parent.ParentId,
                    FirstName = parent.FirstName,
                    MiddleName = parent.MiddleName,
                    LastName = parent.LastName,
                    PhoneNumber = parent.Phone,
                    Email = parent.Email
                });
            }

            return Ok(new
                {
                    Status = true,
                    Message = "Retrieved parents successfully.",
                    Data = new
                    {
                        ParentsList = parentsList,
                        TotalCount = totalCount
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
                Data = (object[])null,
                Errors = new[] { ex.Message }
            });
        }
    }
    [EnableRateLimiting("fixed")] 
    [HasPermission("Parent:update")]
    [HttpPatch("update-parent")]
    public async Task<IActionResult> UpdateParent(UpdateParentRequest request)
    { 
        try
        {
            var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);
            
            await _updateParentUseCase.UpdateParentAsync(new ParentEntry
            {
                ParentId = request.ParentId,
                SchoolId = schoolId,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Phone = request.PhoneNumber,
                Email = request.Email
            });
            
            return Ok(new
            {
                Status = true,
                Message = "Parent updated successfully.",
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

    [HasPermission("Parent:delete")]
    [HttpDelete("delete-parent/{parentId}")]
    public async Task<IActionResult> DeleteParent(int parentId)
    {
        try
        {
            var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

            await _deleteParentUseCase.DeleteParentAsync(schoolId, parentId);

            return Ok(new
            {
                Status = true,
                Message = "Parent deleted successfully.",
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