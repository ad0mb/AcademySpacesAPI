using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IClassroomRepository
{
    Task CreateClassroomAsync(ClassroomEntry classroomEntry);
    Task<(List<ClassroomEntry> classroomsList, int totalCount)> GetClassroomsAsync(int schoolId, int cycleId, int pageSize,
        int pageNumber, string? searchTerm);

    Task UpdateClassroomScheduleAsync(int cycleId, int classroomId, List<int> periodId);
    Task UpdateClassroomRosterAsync(int schoolId, int cycleId, int classroomId, List<int> studentIds);
}