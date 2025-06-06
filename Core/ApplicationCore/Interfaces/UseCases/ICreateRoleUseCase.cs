using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface ICreateRoleUseCase
{
    Task CreateRoleAsync(int schoolId, string roleName, string? roleDescription, List<RolePermissionEntry> permissions);
}