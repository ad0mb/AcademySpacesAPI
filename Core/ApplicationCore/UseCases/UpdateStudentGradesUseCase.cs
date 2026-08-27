using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class UpdateStudentGradesUseCase : IUpdateStudentGradesUseCase
{
    private readonly IStudentGradeRepository _studentGradeRepository;

    public UpdateStudentGradesUseCase(IStudentGradeRepository studentGradeRepository)
    {
        _studentGradeRepository = studentGradeRepository;
    }

    public async Task UpdateStudentGradesAsync(int assignmentId, int periodId, List<StudentGradeEntry> entries)
    {
        await _studentGradeRepository.UpdateStudentGradesAsync(assignmentId, periodId, entries);
    }
}
