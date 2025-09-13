using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class GetClassroomRosterUseCase : IGetClassroomRosterUseCase
{
    
    private readonly IStudentRepository _studentRepository;

    //TODO: Maybe make update classroom roster and get classroom roster students and call it get studnts by classId 
    //TODO: Make frontend call get students and pass in classroomId as filter instead of making a new use case and endpoint to get the roster
    public GetClassroomRosterUseCase(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<List<StudentEntry>> GetClassroomRosterAsync(int schoolId, int cycleId, int classroomId)
    {
        var (students, totalCount) = await _studentRepository.GetStudentsAsync(schoolId, cycleId, 0, 0, null, 0, classroomId, 0);
        
        return students;
    }
    
}