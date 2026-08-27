using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class CreateStudentGradesUseCase : ICreateStudentGradesUseCase
{
    private readonly IStudentGradeRepository _studentGradeRepository;

    public CreateStudentGradesUseCase(IStudentGradeRepository studentGradeRepository)
    {
        _studentGradeRepository = studentGradeRepository;
    }

    public async Task CreateStudentGradesAsync(int assignmentId, int periodId, List<StudentGradeEntry> entries)
    {
        await _studentGradeRepository.CreateStudentGradesAsync(assignmentId, periodId, entries);
    }
}
