using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IRoleRepository
{
    Task<int> CreateRoleAsync(RoleEntry role);
    Task<int> CreateRoleAsync(RoleEntry role, string[] permissions);
    Task<List<RoleEntry>> GetRolesBySchoolIdAsync(int schoolId);
    Task UpdateRoleAsync(RoleEntry request);
}