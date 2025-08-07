using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetCoursesUseCase
{
    Task<(List<CourseEntry> courseList, int totalCount)> GetCoursesAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm);
}