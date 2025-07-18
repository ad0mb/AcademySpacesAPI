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

    public async Task<List<CourseEntry>> GetCoursesAsync(int schoolId)
    {

        var courses = await _courseRepository.GetCoursesBySchoolIdAsync(schoolId);

        return courses;
    }
}