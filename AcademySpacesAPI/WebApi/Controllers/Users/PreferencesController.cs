using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;
using AcademySpacesAPI.Exceptions;
using AcademySpacesAPI.WebApi.DTOs.Requests;
using AcademySpacesAPI.WebApi.DTOs.Responses;
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
            var data = await _getUserPreferencesUseCase.GetUserPreferencesAsync();
            
            var response = new GetUserPreferencesResponse
            {
                PageBrightness = data.PageBrightness,
                Locale = data.Locale
            };

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
            var data = await _postUserPreferencesUseCase.PostUserPreferencesAsync(new WebPreferencesEntry
            {
                PageBrightness = request.PageBrightness,
                Locale = request.Locale
            });

            var response = new GetUserPreferencesResponse
            {
                PageBrightness = data.PageBrightness,
                Locale = data.Locale
            };

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