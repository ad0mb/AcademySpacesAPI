using AcademySpacesAPI.ApplicationCore.DomainEntities;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

public interface IFacultyRepository
{
    Task<int> CreateFacultyAsync(FacultyEntry faculty);
    Task<int> CreateFacultyAsync(FacultyEntry facultyEntry, int[] roleIds);
    Task AddRoleToFacultyAsync(FacultyRoleEntry facultyRole);
    Task<FacultyEntry?> GetFacultyByIdentityIdAsync(string identityId);
}