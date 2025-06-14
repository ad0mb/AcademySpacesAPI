using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetFacultyUseCase
{
    Task<List<FacultyEntry>> GetFacultyAsync(int schoolId);
}