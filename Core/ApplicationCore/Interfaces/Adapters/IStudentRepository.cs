using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IStudentRepository
{
    Task<(List<StudentEntry> studentList, int totalCount )> GetStudentsAsync(int schoolId, int cycleId, int pageSize, int pageNumber, string? searchTerm, int yearLevelId, int classroomId, bool noClassroom = false);
    Task<int> CreateStudentAsync(StudentEntry student);
    Task UpdateStudentAsync(StudentEntry student);
}