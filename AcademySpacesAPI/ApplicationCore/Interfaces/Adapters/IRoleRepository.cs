using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.Entities;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

public interface IRoleRepository
{
    Task<int> CreateRoleAsync(CreateRoleEntry role);
    Task<int> CreateRoleAsync(CreateRoleEntry role, string[] permissions);
}