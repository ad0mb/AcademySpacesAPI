using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetClassroomsUseCase
{
    Task<List<ClassroomEntry>> GetClassroomsAsync(int schoolId);
}