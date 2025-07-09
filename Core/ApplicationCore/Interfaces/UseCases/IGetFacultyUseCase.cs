using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetFacultyUseCase
{
    Task<List<FacultyEntry>> GetFacultyAsync(int schoolId);
    Task<(List<FacultyEntry> facultyList, int totalCount)> GetFacultyAsync(int schoolId, int pageSize, int pageNumber, string? sortBy);
}