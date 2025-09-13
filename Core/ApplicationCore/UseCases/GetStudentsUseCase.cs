using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class GetStudentsUseCase : IGetStudentsUseCase
{
    
    private readonly IStudentRepository _studentRepository;
    
    public GetStudentsUseCase(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
    
    public async Task<(List<StudentEntry> studentList, int totalCount)> GetStudentsAsync(int schoolId, int cycleId, int pageSize, int pageNumber, string? searchTerm, int yearLevelId, int classroomId, int periodId, bool noClassroom = false)
    {
        var (students, totalCount) = await _studentRepository.GetStudentsAsync(schoolId, cycleId, pageSize, pageNumber, searchTerm, yearLevelId, classroomId, periodId, noClassroom);

        return (students, totalCount);
    }
}