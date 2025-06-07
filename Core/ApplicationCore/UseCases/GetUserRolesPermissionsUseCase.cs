using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class GetUserRolesPermissionsUseCase : IGetUserRolesPermissionsUseCase
{

    private readonly IPermissionsRepository _permissionsRepository;

    public GetUserRolesPermissionsUseCase(IPermissionsRepository permissionsRepository)
    {
        _permissionsRepository = permissionsRepository;
    }

    public async Task<List<RolePermissionEntry>?> GetUserPermissionsByRoleIdAsync(int roleId)
    {
        var permissions = await _permissionsRepository.GetUserPermissionsByRoleIdAsync(roleId);

        return permissions;
    }
}