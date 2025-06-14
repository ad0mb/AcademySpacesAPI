using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetUserRolesPermissionsUseCase
{
    Task<List<RolePermissionEntry>?> GetUserPermissionsByRoleIdAsync(int roleId);
}