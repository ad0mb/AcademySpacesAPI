using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface ICreateCourseUseCase
{
    Task CreateCourseAsync(CourseEntry courseEntry);
}