using AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;
using AcademySpacesAPI.Exceptions;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademySpacesAPI.WebApi.Controllers.Users;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/user/")]
public class PreferencesController : ControllerBase
{
    
    private readonly IGetUserPreferencesUseCase _getUserPreferencesUseCase;
    private readonly IPostUserPreferencesUseCase _postUserPreferencesUseCase;
    
    public PreferencesController(IGetUserPreferencesUseCase getUserPreferencesUseCase, IPostUserPreferencesUseCase postUserPreferencesUseCase)
    {
        _getUserPreferencesUseCase = getUserPreferencesUseCase;
        _postUserPreferencesUseCase = postUserPreferencesUseCase;
    }

    [HttpGet("preferences")]
    public async Task<IActionResult> GetUserPreferences()
    {
        try
        {
            var response = await _getUserPreferencesUseCase.GetUserPreferencesAsync();

            return Ok(new
            {
                Status = true,
                Message = "User preferences retrieved successfully.",
                Data = response,
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
    }

    [HttpPost("preferences")]
    public async Task<IActionResult> PostUserPreferences(PostUserPreferencesRequest request)
    {
        try
        {
            var response = await _postUserPreferencesUseCase.PostUserPreferencesAsync(request);

            return Ok(new
            {
                Status = true,
                Message = "User preferences updated successfully.",
                Data = response,
                Errors = (string[])null
            });

        } catch (DbException ex)
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
}