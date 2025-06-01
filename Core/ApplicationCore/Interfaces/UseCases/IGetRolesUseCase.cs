using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetRolesUseCase
{
    Task<List<RoleEntry>> GetRolesAsync(int schoolId);
}