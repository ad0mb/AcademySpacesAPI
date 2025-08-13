using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetClassroomsUseCase
{
    Task<(List<ClassroomEntry> classrooms, int totalCount)> GetClassroomsAsync(int schoolId, int cycleId, int pageSize, int pageNumber, string? searchTerm);
}