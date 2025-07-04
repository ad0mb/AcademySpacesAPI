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
    
    public async Task<List<StudentEntry>> GetStudentsAsync(int schoolId)
    {
        var students = await _studentRepository.GetStudentsAsync(schoolId);

        return students;
    }
}