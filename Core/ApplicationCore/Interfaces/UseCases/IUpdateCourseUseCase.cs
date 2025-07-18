using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IUpdateCourseUseCase
{
    Task UpdateCourseAsync(CourseEntry courseEntry);
}