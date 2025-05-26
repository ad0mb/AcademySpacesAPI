using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface ISchoolRepository
{
    Task<int[]> CreateSchoolAsync(SchoolEntry school);
}