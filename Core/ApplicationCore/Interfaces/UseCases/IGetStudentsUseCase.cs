using Core.ApplicationCore.DomainEntities;

namespace Core.ApplicationCore.Interfaces.UseCases;

public interface IGetStudentsUseCase
{
    Task <List<StudentEntry>> GetStudentsAsync(int schoolId);
}