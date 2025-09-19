using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class DeleteFacultyUseCase : IDeleteFacultyUseCase
{
    
    private readonly IFacultyRepository _facultyRepository;
    
    public DeleteFacultyUseCase(IFacultyRepository facultyRepository)
    {
        _facultyRepository = facultyRepository;
    }

    public async Task DeleteFacultyAsync(int schoolId, int facultyId)
    {
        await _facultyRepository.DeleteFacultyAsync(schoolId, facultyId);
    }
}