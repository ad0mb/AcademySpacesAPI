using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class SetStudentsGradesUseCase : ISetStudentsGradesUseCase
{
    
    private readonly IStudentGradeRepository _studentGradeRepository;
    
    public SetStudentsGradesUseCase(IStudentGradeRepository studentGradeRepository)
    {
        _studentGradeRepository = studentGradeRepository;
    }

    public async Task SetStudentsGradesAsync(int schoolId, int cycleId, int periodId, List<StudentGradeEntry> gradeEntries)
    {
        await _studentGradeRepository.SetStudentsGradesAsync(schoolId, cycleId, periodId, gradeEntries);
    }
}