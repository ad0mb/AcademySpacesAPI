using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetClassroomRosterUseCase
{
    Task<List<StudentEntry>> GetClassroomRosterAsync(int schoolId, int cycleId, int classroomId);
}