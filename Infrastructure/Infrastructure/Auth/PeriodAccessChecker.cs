using System.Security.Claims;
using System.Text.Json;
using Infrastructure.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Auth;

public class PeriodAccessChecker
{
    
    private readonly MyDbContext _context;

    public PeriodAccessChecker(MyDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> CheckAccess(ClaimsPrincipal principal, int periodId, string permission)
    {
        var isAdminOrHasPeriodAccess = HasAdminOrPeriodAccess(principal, permission); //permissions checking
        bool isPeriodMember = false;
        
        if (!isAdminOrHasPeriodAccess)
        {
            //TODO: Move this to a repository
            isPeriodMember = await (from p in _context.Periods
                where 
                    p.PeriodId == periodId && 
                    p.Teacher.IdentityId == principal.FindFirst(ClaimTypes.NameIdentifier).Value
                select p).AnyAsync();
        }
        
        if (!isAdminOrHasPeriodAccess && !isPeriodMember)
        {
            return false;
        }

        return true;
    }
    
    private bool HasAdminOrPeriodAccess(ClaimsPrincipal principal, string permission)
    {
        foreach (var claim in principal.Claims)
        {
            if (claim.Type == "permission")
            {
                var permissionJson = JsonSerializer.Deserialize<JsonElement>(claim.Value);
                
                if (permissionJson.GetProperty("PermissionName").GetString() == "administrator" ||
                    permissionJson.GetProperty("PermissionName").GetString() == "chiefadministrator")
                {
                    return true;
                }
                
                if (permissionJson.GetProperty("PermissionName").GetString() == "periods")
                {
                    switch (permission)
                    {
                        case "view":
                            return true;
                            break;
                        
                        case "create":
                            if (permissionJson.GetProperty("Create").GetBoolean())
                            {
                                return true;
                            }
                            break;
                        
                        case "delete":
                            if (permissionJson.GetProperty("Delete").GetBoolean())
                            {
                                return true;
                            }
                            break;
                        
                        case "update":
                            if (permissionJson.GetProperty("Update").GetBoolean())
                            {
                                return true;
                            }
                            break;
                    }
                }
            }
        }
        
        return false;
    }
}