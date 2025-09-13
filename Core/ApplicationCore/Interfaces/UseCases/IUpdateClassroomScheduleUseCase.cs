namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IUpdateClassroomScheduleUseCase
{
    Task UpdateClassroomScheduleAsync(int cycleId, int classroomId, List<int> periodId);
}