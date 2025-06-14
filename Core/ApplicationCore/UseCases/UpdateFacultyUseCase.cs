using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class UpdateFacultyUseCase : IUpdateFacultyUseCase
{

    private readonly IFacultyRepository _facultyRepository;
    
    public UpdateFacultyUseCase(IFacultyRepository facultyRepository)
    {
        _facultyRepository = facultyRepository;
        
    }

    public async Task UpdateFacultyAsync(FacultyEntry facultyEntry)
    {

        await _facultyRepository.UpdateFacultyAsync(facultyEntry);
    }
}