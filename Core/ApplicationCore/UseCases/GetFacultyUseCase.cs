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
    
    public async Task<List<FacultyEntry>> GetFacultyAsync(int schoolId)
    {

        var faculty = await _facultyRepository.GetFacultyBySchoolIdAsync(schoolId);

        return faculty;
    }

    public async Task<(List<FacultyEntry> facultyList, int totalCount)> GetFacultyAsync(int schoolId, int pageSize, int pageNumber, string? sortBy)
    {
        var faculty = await _facultyRepository.GetFacultyBySchoolIdAsync(schoolId, pageSize, pageNumber);
        
        return (faculty.facultyList, faculty.totalCount);
    }
}