using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IUpdateStudentUseCase
{
    Task UpdateStudentAsync(StudentEntry request);
}