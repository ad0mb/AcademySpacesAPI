using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.ApplicationCore.UseCases;

public class CreatePermissionsJwtUseCase : ICreatePermissionsJwtUseCase
{
    
    private readonly IConfiguration _configuration;
    
    public CreatePermissionsJwtUseCase(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> CreatePermissionsJwtTokenAsync(IEnumerable<Claim> claims)
    {
        var permissions = new List<Claim>();
        var tokenHandler = new JwtSecurityTokenHandler();
        
        foreach (var claim in claims)
        {
            if (claim.Type == "permission")
            {
                var deserializedPermission = JsonSerializer.Deserialize<JsonElement>(claim.Value);
                permissions.Add(new Claim("permission", JsonSerializer.Serialize(new SanitizedUserPermissions
                {
                    Name = deserializedPermission.GetProperty("PermissionName").GetString(),
                    Create = deserializedPermission.GetProperty("Create").GetBoolean().ToString(),
                    Delete = deserializedPermission.GetProperty("Delete").GetBoolean().ToString(),
                    Update = deserializedPermission.GetProperty("Update").GetBoolean().ToString()
                })));
            }
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtBearer:Issuer"],
            audience: _configuration["JwtBearer:Audience"],
            claims: permissions,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtBearer:PermsRegistration:Key"])),
                SecurityAlgorithms.HmacSha256)
        );

        return tokenHandler.WriteToken(token);
    }
}