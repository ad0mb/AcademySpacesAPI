using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class GetStudentsGradesUseCase : IGetStudentGradesUseCase
{
    
    private readonly IStudentGradeRepository _studentGradeRepository;

    public GetStudentsGradesUseCase(IStudentGradeRepository studentGradeRepository)
    {
        _studentGradeRepository = studentGradeRepository;
    }
    
    public async Task<List<StudentGradeEntry>> GetStudentsGradesAsync(int cycleId, int assignmentId, int periodId)
    {
        var studentGrades = await _studentGradeRepository.GetStudentsGradesAsync(cycleId, assignmentId, periodId);
        
        return studentGrades;
    }
}