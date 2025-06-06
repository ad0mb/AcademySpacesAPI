using System.Data.Common;
using System.Security.Claims;
using System.Text.Json;
using AcademySpacesAPI.WebApi.DTOs.Responses;
using Core;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DbException = Core.Exceptions.DbException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AcademySpacesAPI.WebApi.Controllers.Users;

[ApiController]
[Authorize(AuthenticationSchemes = "FirebaseAuthScheme")]
[Route("api/user/")]
public class PermissionsController : ControllerBase
{
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICreatePermissionsJwtUseCase _createPermissionsJwtUseCase;
    private readonly IConfiguration _configuration;
    
    public PermissionsController(IHttpContextAccessor httpContextAccessor, ICreatePermissionsJwtUseCase createPermissionsJwtUseCase, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _createPermissionsJwtUseCase = createPermissionsJwtUseCase;
    }
    
    [HttpGet("permissions")]
    public async Task<IActionResult> GetUserPermissions()
    {
        var claims = _httpContextAccessor.HttpContext.User.Claims;

        try
        {
            var token = await _createPermissionsJwtUseCase.CreatePermissionsJwtTokenAsync(claims);
            
            _httpContextAccessor.HttpContext.Response.Cookies.Append("perms", token, new CookieOptions
            {
                HttpOnly = _configuration["Environment"] == "Production",
                Secure = _configuration["Environment"] == "Production",
                SameSite = SameSiteMode.Strict,
                Path = "/"
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

        return Ok(new
        {
            Status = true,
            Message = "Retrieved user permissions successfully.",
            Data = (object[])null,
            Errors = (string[])null
        });
    }
}