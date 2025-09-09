using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class CreateStudentUseCase : ICreateStudentUseCase
{
    
    private readonly IStudentRepository _studentRepository;
    private readonly IParentRepository _parentRepository;

    public CreateStudentUseCase(IStudentRepository studentRepository, IParentRepository parentRepository)
    {
        _studentRepository = studentRepository;
        _parentRepository = parentRepository;
    }

    public async Task CreateStudentAsync(StudentEntry request)
    {
        var studentId = await _studentRepository.CreateStudentAsync(request);

        await _parentRepository.BulkLinkStudentToParentAsync(request.ParentIds, studentId);
    }
}