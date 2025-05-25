using AcademySpacesAPI.ApplicationCore.DomainEntities;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

public interface IPermissionsRepository
{
    Task CreateRolePermissionAsync(CreateRolePermissionEntry rolePermissions);
    Task<List<RolePermissionEntry>?> GetUserPermissionsByIdentityIdAsync(string identityId, string userType);
}