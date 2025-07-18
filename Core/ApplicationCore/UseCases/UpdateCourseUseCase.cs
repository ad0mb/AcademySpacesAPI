using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class UpdateCourseUseCase : IUpdateCourseUseCase
{
    
    private readonly ICourseRepository _courseRepository;

    public UpdateCourseUseCase(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task UpdateCourseAsync(CourseEntry courseEntry)
    {
        await _courseRepository.UpdateCourseAsync(courseEntry);
    }
}