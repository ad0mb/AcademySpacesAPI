using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.Infrastructure.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AcademySpacesAPI.WebApi.Controllers.Developers;

[ApiController]
[Route("api/developers/invite")]
public class DevInviteController : ControllerBase
{
    
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    //TODO: Add special auth scheme for dev collection of endpoints
    public DevInviteController(IConfiguration configuration, IEmailService emailService)
    {
        _configuration = configuration;
        _emailService = emailService;
    }
    
    [HttpPost("invite-admin/{email}")]
    public IActionResult InviteAdmin(string email)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtBearer:Issuer"],
            audience: _configuration["JwtBearer:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["JwtBearer:SchoolRegistration:ExpiryInHours"])),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtBearer:SchoolRegistration:Key"])), SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        
        _emailService.SendSchoolRegistrationEmailAsync(email, tokenString);
        
        return Ok(new
        {
            Status = true,
            Message = "Invitation sent successfully.",
            Data = new { email },
            Errors = (string[])null
        });
    }
}