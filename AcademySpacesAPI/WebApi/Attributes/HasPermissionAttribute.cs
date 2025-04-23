using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AcademySpacesAPI.Webapi.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class HasPermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string _permissionName;
    private readonly string _permission;

    public HasPermissionAttribute(string permissionObject)
    {
        _permissionName = permissionObject.Split(":")[0];
        _permission = permissionObject.Split(":")[1];
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var foundPermission = false; //counts as view permission since view is based off permission existence
        var isAdmin = false; 
        
        foreach (var claim in context.HttpContext.User.Claims)
        {
            if (claim.Type == "permission")
            {
                var permissionJson = JsonSerializer.Deserialize<JsonElement>(claim.Value);

                if (permissionJson.GetProperty("PermissionName").GetString() == "Administrator" ||
                    permissionJson.GetProperty("PermissionName").GetString() == "ChiefAdministrator")
                {
                    isAdmin = true;
                }

                if (_permissionName == permissionJson.GetProperty("PermissionName").GetString())
                {
                    switch (_permission)
                    {
                        case "view":
                            foundPermission = true;
                            break;
                        
                        case "create":
                            if (permissionJson.GetProperty("Create").GetBoolean())
                            {
                                foundPermission = true;
                            }
                            break;
                        
                        case "delete":
                            if (permissionJson.GetProperty("Delete").GetBoolean())
                            {
                                foundPermission = true;
                            }
                            break;
                        
                        case "update":
                            if (permissionJson.GetProperty("Update").GetBoolean())
                            {
                                foundPermission = true;
                            }
                            break;
                    }
                }
            }
        }
        
        if (!foundPermission && !isAdmin)
        {
            context.Result = new JsonResult(new
                {
                    message = "Insufficient permissions for request",
                    RequiredPermission = _permissionName+":"+_permission
                })
                {StatusCode = 403};
        }
    }
}