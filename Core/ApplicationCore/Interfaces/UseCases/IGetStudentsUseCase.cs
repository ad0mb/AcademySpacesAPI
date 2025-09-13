using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetStudentsUseCase
{
    Task <(List<StudentEntry> studentList, int totalCount )> GetStudentsAsync(int schoolId, int cycleId, int pageSize, int pageNumber, string? searchTerm, int yearLevelId, int classroomId, int periodId, bool noClassroom = false);
}