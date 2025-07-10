using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface IClassroomRepository
{
    Task CreateClassroomAsync(ClassroomEntry classroomEntry);
    Task<List<ClassroomEntry>> GetClassroomsAsync(int schoolId);
}