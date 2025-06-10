using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;

namespace Core.ApplicationCore.UseCases;

public class UpdateRoleUseCase : IUpdateRoleUseCase
{
    
    private readonly IPermissionsRepository _permissionsRepository;
    private readonly IRoleRepository _roleRepository;
    
    public UpdateRoleUseCase(IRoleRepository roleRepository, IPermissionsRepository permissionsRepository)
    {
        _permissionsRepository = permissionsRepository;
        _roleRepository = roleRepository;
    }

    public async Task UpdateRoleAsync(RoleEntry request, List<RolePermissionEntry> permissions, List<int> permissionsToDelete)
    {
        await _roleRepository.UpdateRoleAsync(request);

        await _permissionsRepository.BulkUpdateOrInsertRolePermissionAsync(permissions);
        
        await _permissionsRepository.DeleteRolePermissionAsync(permissionsToDelete);
    }
}