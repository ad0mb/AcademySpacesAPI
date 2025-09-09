using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class UpdateStudentUseCase : IUpdateStudentUseCase
{
    
    private readonly IStudentRepository _studentRepository;
    private readonly IParentRepository _parentRepository;

    public UpdateStudentUseCase(IStudentRepository studentRepository, IParentRepository parentRepository)
    {
        _studentRepository = studentRepository;
        _parentRepository = parentRepository;
    }

    public async Task UpdateStudentAsync(StudentEntry request)
    {
        await _studentRepository.UpdateStudentAsync(request);

        await _parentRepository.BulkLinkStudentToParentAsync(request.ParentIds, request.StudentId);
    }
}