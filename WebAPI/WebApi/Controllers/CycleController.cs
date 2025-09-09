using AcademySpacesAPI.WebApi.Attributes;
using AcademySpacesAPI.WebApi.DTOs.Responses;
using Core.ApplicationCore.Interfaces.UseCases;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/school/cycles")]
public class CycleController : ControllerBase
{
    
    private readonly IGetCyclesUseCase _getCyclesUseCase;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CycleController(IGetCyclesUseCase getCyclesUseCase, IHttpContextAccessor httpContextAccessor)
    {
        _getCyclesUseCase = getCyclesUseCase;
        _httpContextAccessor = httpContextAccessor;
    }

    [HasPermission("Cycle:view")]
    [HttpGet("get-cycles")]
    public async Task<IActionResult> GetCyclesAsync([FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? searchTerm)
    {

        var cycles = new List<GetCyclesResponse>();
        
        try
        {
            var schoolId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("school_id").Value);

            var (cyclesList, totalCount) = await _getCyclesUseCase.GetCyclesAsync(schoolId, pageSize, pageNumber, searchTerm);

            foreach (var cycle in cyclesList)
            {
                cycles.Add(new GetCyclesResponse
                {
                    CycleId = cycle.CycleId,
                    IsActive = cycle.IsActive,
                    isArchived = cycle.isArchived,
                    CycleName = cycle.CycleName,
                    Code = cycle.Code,
                    ScheduleType = cycle.ScheduleType,
                    StartDate = cycle.StartDate,
                    EndDate = cycle.EndDate,
                    GradingPeriods = cycle.GradingPeriods.Select(gp => new GetGradingPeriodsResponse
                    {
                        GradingPeriodId = gp.GradingPeriodId,
                        StartDate = gp.StartDate,
                        EndDate = gp.EndDate
                    }).ToList(),
                    IsExpired = cycle.IsExpired
                });
            }

            return Ok(new
                {
                    Status = true,
                    Message = "Retrieved cycles successfully.",
                    Data = new
                    {
                        CyclesList = cycles,
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

    [HasPermission("Cycle:create")]
    [HttpPost("create-cycle")]
    public async Task<IActionResult> CreateCyclesAsync()
    {
        throw new NotImplementedException();
    }
}