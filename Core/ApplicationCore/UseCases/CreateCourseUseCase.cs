using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class CreateCourseUseCase : ICreateCourseUseCase
{
    
    private readonly ICourseRepository _courseRepository;

    public CreateCourseUseCase(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task CreateCourseAsync(CourseEntry courseEntry)
    {
        await _courseRepository.CreateCourseAsync(courseEntry);
    }
}