using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class DeleteCourseUseCase : IDeleteCourseUseCase
{
    
    private readonly ICourseRepository _courseRepository;
    
    public DeleteCourseUseCase(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task DeleteCourseAsync(int schoolId, int courseId)
    {
        await _courseRepository.DeleteCourseAsync(schoolId, courseId);
    }
}