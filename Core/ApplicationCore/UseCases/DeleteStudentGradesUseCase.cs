using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class DeleteStudentGradesUseCase : IDeleteStudentGradesUseCase
{
    private readonly IStudentGradeRepository _studentGradeRepository;

    public DeleteStudentGradesUseCase(IStudentGradeRepository studentGradeRepository)
    {
        _studentGradeRepository = studentGradeRepository;
    }

    public async Task DeleteStudentGradesAsync(int assignmentId, int periodId, List<int> studentIds)
    {
        await _studentGradeRepository.DeleteStudentGradesAsync(assignmentId, periodId, studentIds);
    }
}
