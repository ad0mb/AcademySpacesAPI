using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.Entities;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

public interface ISchoolRepository
{
    Task<int[]> CreateSchoolAsync(CreateSchoolEntry school);
}