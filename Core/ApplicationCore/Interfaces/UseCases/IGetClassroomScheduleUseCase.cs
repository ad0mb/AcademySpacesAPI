using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetClassroomScheduleUseCase
{
    Task<List<PeriodEntry>> GetClassroomScheduleAsync(int classroomId, int cycleId);
}