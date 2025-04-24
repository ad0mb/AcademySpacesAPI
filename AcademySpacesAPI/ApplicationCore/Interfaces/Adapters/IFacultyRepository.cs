using AcademySpacesAPI.ApplicationCore.DomainEntities;

namespace AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;

public interface IFacultyRepository
{
    Task<int> CreateFacultyAsync(CreateFacultyEntry faculty);
    Task<int> CreateFacultyAsync(CreateFacultyEntry facultyEntry, int[] roleIds);
    Task AddRoleToFacultyAsync(CreateFacultyRoleEntry facultyRole);
}