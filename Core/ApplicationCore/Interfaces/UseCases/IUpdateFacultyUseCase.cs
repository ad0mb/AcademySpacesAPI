using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IUpdateFacultyUseCase
{
    //TODO: Add roleIds parameter when we decide to implement role management within the faculty update dialog.
    Task UpdateFacultyAsync(FacultyEntry facultyEntry);
}