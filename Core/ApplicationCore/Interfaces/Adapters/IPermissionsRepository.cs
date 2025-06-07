using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IPermissionsRepository
{
    Task CreateRolePermissionAsync(RolePermissionEntry rolePermissions);
    Task<List<RolePermissionEntry>?> GetUserPermissionsByIdentityIdAsync(string identityId, string userType);
    Task<List<RolePermissionEntry>?> GetUserPermissionsByRoleIdAsync(int RoleId);
}