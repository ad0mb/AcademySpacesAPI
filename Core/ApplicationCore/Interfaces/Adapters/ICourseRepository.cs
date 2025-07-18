using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.Adapters;

public interface ICourseRepository
{
    Task<List<CourseEntry>> GetCoursesBySchoolIdAsync(int schoolId);
    Task CreateCourseAsync(CourseEntry courseEntry);
}