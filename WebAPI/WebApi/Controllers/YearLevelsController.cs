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
[Route("api/school/settings/year-levels")]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
public class YearLevelsController : ControllerBase
{
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGetYearLevelHierarchyUseCase _getYearLevelHierarchyUseCase;
    private readonly ISetYearLevelHierarchyUseCase _setYearLevelHierarchyUseCase;
    
    public YearLevelsController(IHttpContextAccessor httpContextAccessor, IGetYearLevelHierarchyUseCase getYearLevelHierarchyUseCase, ISetYearLevelHierarchyUseCase setYearLevelHierarchyUseCase)
    {
        _httpContextAccessor = httpContextAccessor;
        _getYearLevelHierarchyUseCase = getYearLevelHierarchyUseCase;
        _setYearLevelHierarchyUseCase = setYearLevelHierarchyUseCase;
    }
    [EnableRateLimiting("fixed")] 
    [HasPermission("YearLevels:view")]
    [HttpGet("get-year-levels")]
    public async Task<IActionResult> GetYearLevels()
    {
        try
        {
            var result = new List<GetYearLevelsResponse>();

            var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

            var yearLevels = await _getYearLevelHierarchyUseCase.GetYearLevelHierarchyAsync(schoolId);

            foreach (var yearLevel in yearLevels)
            {
                result.Add(new GetYearLevelsResponse
                {
                    Id = yearLevel.Id,
                    YearLevelName = yearLevel.YearLevelName,
                    YearLevelCode = yearLevel.YearLevelCode,
                    Description = yearLevel.Description,
                });
            }

            return Ok(new
            {
                Status = true,
                Message = "Retrieved year levels successfully.",
                Data = result,
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
    [HasPermission("YearLevels:create")]
    [HasPermission("YearLevels:update")]
    [HttpPost("set-year-levels")]
    public async Task<IActionResult> SetYearLevels(List<SetYearLevelsRequest> request)
    {
        
        var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);
        
        try
        {
            var yearLevels = new List<YearLevelEntry>();
            
            var yearLevelsToDelete = new List<int>();
            
            foreach (var yearLevel in request)
            {
                if (yearLevel.toDelete == true)
                {
                    yearLevelsToDelete.Add(yearLevel.Id);
                }
                else
                {
                    yearLevels.Add(new YearLevelEntry
                    {
                        Id = yearLevel.Id,
                        SchoolId = schoolId,
                        YearLevelName = yearLevel.YearLevelName,
                        YearLevelCode = yearLevel.YearLevelCode,
                        Description = yearLevel.Description,
                    });
                }
            }

            await _setYearLevelHierarchyUseCase.SetYearLevelHierarchyAsync(yearLevels, yearLevelsToDelete);
            
            return Ok(new
            {
                Status = true,
                Message = "Year levels updated successfully.",
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
}