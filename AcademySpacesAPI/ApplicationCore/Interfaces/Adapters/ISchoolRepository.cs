using AcademySpacesAPI.ApplicationCore.DomainEntities;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

public interface ISchoolRepository
{
    Task<int[]> CreateSchoolAsync(CreateSchoolEntry school);
}