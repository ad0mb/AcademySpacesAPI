using AcademySpacesAPI.ApplicationCore.DomainEntities;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

public interface IRoleRepository
{
    Task<int> CreateRoleAsync(CreateRoleEntry role);
    Task<int> CreateRoleAsync(CreateRoleEntry role, string[] permissions);
}