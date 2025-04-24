using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.ApplicationCore.Interfaces.UseCases;
using AcademySpacesAPI.Entities;
using AcademySpacesAPI.WebApi.DTOs;

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

    //TODO: Handle exception and return exception meant for api controllers
    public async Task CreateSchoolAndAdminAsync(RegisterSchoolRequest request)
    {
        var ids = await _schoolRepository.CreateSchoolAsync(new CreateSchoolEntry
        {
            SchoolName = request.SchoolName,
            SchoolCountry = request.SchoolCountry
        });
        
        var facultyId = await _facultyRepository.CreateFacultyAsync(new CreateFacultyEntry
        {
            SchoolId = ids[0],
            IdentityId = request.IdentityId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.SigninEmail,
        }, [ids[1]]);
    }
}