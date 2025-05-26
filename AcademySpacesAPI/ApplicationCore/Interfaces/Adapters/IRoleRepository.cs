using AcademySpacesAPI.ApplicationCore.DomainEntities;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

public interface IRoleRepository
{
    Task<int> CreateRoleAsync(RoleEntry role);
    Task<int> CreateRoleAsync(RoleEntry role, string[] permissions);
}