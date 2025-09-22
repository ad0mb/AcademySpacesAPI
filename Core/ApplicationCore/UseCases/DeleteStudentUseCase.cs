using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class DeleteStudentUseCase : IDeleteStudentUseCase
{
    
    private readonly IStudentRepository _studentRepository;
    
    public DeleteStudentUseCase(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task DeleteStudentAsync(int schoolId, int studentId)
    {
        await _studentRepository.DeleteStudentAsync(schoolId, studentId);
    }
}