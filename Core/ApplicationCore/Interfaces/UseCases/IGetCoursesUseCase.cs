using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetCoursesUseCase
{
    Task<List<CourseEntry>> GetCoursesAsync(int schoolId);
}