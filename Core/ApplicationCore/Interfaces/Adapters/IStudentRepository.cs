using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IStudentRepository
{
    Task<(List<StudentEntry> studentList, int totalCount )> GetStudentsAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm, int yearLevelId);
    Task<int> CreateStudentAsync(StudentEntry student);
}