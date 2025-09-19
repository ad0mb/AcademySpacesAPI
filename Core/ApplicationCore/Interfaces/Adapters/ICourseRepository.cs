using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface ICourseRepository
{
    Task<(List<CourseEntry> courseList, int totalCount)> GetCoursesBySchoolIdAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm);
    Task CreateCourseAsync(CourseEntry courseEntry);
    Task UpdateCourseAsync(CourseEntry courseEntry);
    Task DeleteCourseAsync(int schoolId, int courseId);
}