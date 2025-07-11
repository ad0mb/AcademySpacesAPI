using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

public class GetFacultyUseCase : IGetFacultyUseCase
{

    private readonly IFacultyRepository _facultyRepository;
    
    public GetFacultyUseCase(IFacultyRepository facultyRepository)
    {
        _facultyRepository = facultyRepository;
    }
    
    public async Task<(List<FacultyEntry> facultyList, int totalCount)> GetFacultyAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm)
    {
        var faculty = await _facultyRepository.GetFacultyBySchoolIdAsync(schoolId, pageSize, pageNumber, searchTerm);
        
        return (faculty.facultyList, faculty.totalCount);
    }
}