using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;
using AcademySpacesAPI.WebApi.DTOs.Requests;

namespace AcademySpacesAPI.ApplicationCore.UseCases;

public class RegisterSchoolAndAdminUseCase : IRegisterSchoolAndAdminUseCase
{
    
    private readonly ISchoolRepository _schoolRepository;
    private readonly IFacultyRepository _facultyRepository;
    
    public RegisterSchoolAndAdminUseCase(ISchoolRepository schoolRepository, IFacultyRepository facultyRepository)
    {
        _schoolRepository = schoolRepository;
        _facultyRepository = facultyRepository;
    }

    
    //TODO: Handle exception and delete faculty user from firebase if school creation fails or faculty creation fails
    public async Task CreateSchoolAndAdminAsync(RegisterSchoolRequest request)
    {
        var ids = await _schoolRepository.CreateSchoolAsync(new SchoolEntry
        {
            SchoolName = request.SchoolName,
            SchoolCountry = request.SchoolCountry
        });
        
        var facultyId = await _facultyRepository.CreateFacultyAsync(new FacultyEntry
        {
            SchoolId = ids[0],
            IdentityId = request.IdentityId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.SigninEmail,
        }, [ids[1]]);
    }
}