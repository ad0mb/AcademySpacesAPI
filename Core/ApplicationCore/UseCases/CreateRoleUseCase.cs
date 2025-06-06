using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class CreateRoleUseCase : ICreateRoleUseCase
{
    
    private readonly IRoleRepository _roleRepository;
    
    public CreateRoleUseCase(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task CreateRoleAsync(int schoolId, string roleName, string? roleDescription, List<RolePermissionEntry> permissions)
    {
        var permissionStrings = new string[permissions.Count];
        var count = 0;
        foreach (var permission in permissions)
        {
            var permissionString = permission.PermissionName + ":";

            if (permission.Create) permissionString += "1"; else permissionString += "0";
            if (permission.Delete) permissionString += "1"; else permissionString += "0";
            if (permission.Update) permissionString += "1"; else permissionString += "0";
            
            permissionStrings[count] = permissionString;
            count++;
        }
        
        var roleEntry = new RoleEntry
        {
            SchoolId = schoolId,
            RoleName = roleName,
            RoleDescription = roleDescription
        };
        
        await _roleRepository.CreateRoleAsync(roleEntry, permissionStrings);
        
    }
}