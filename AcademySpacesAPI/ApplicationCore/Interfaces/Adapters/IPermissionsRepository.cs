using AcademySpacesAPI.ApplicationCore.DomainEntities;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

public interface IPermissionsRepository
{
    Task CreateRolePermissionAsync(RolePermissionEntry rolePermissions);
    Task<List<RolePermissionEntry>?> GetUserPermissionsByIdentityIdAsync(string identityId, string userType);
}