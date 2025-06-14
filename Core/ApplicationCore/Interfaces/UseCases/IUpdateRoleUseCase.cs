using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.UseCases;

public interface IUpdateRoleUseCase
{
    Task UpdateRoleAsync(RoleEntry request, List<RolePermissionEntry> permissions, List<int> permissionsToDelete);
}