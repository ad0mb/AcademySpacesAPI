namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IUpdateClassroomRosterUseCase
{
    Task UpdateClassroomRoster(int schoolId, int cycleId, int classroomId, List<int> studentIds);
}