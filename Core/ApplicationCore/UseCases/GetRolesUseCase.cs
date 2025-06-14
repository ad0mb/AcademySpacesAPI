using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class GetRolesUseCase : IGetRolesUseCase
{
    
    private readonly IRoleRepository _roleRepository;
    
    public GetRolesUseCase(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }
    public Task<List<RoleEntry>> GetRolesAsync(int schoolId)
    {
        var roles = _roleRepository.GetRolesBySchoolIdAsync(schoolId);
        
        return roles;
    }
}