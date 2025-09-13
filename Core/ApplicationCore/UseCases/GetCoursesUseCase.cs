using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class GetCoursesUseCase : IGetCoursesUseCase
{
    
    private readonly ICourseRepository _courseRepository;
    
    public GetCoursesUseCase(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<(List<CourseEntry> courseList, int totalCount)> GetCoursesAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm)
    {

        var (courseList, totalCount) = await _courseRepository.GetCoursesBySchoolIdAsync(schoolId, pageSize, pageNumber, searchTerm);

        return (courseList, totalCount);
    }
}