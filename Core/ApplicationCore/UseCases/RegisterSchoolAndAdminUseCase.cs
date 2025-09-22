using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.ApplicationCore.Interfaces.UseCases;

namespace Core.ApplicationCore.UseCases;

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
    //TODO: Make sure email is unqiue in faculty table check before.
    public async Task CreateSchoolAndAdminAsync(string schoolName, string schoolCountry, string firstName, string lastName, string signinEmail, string identityId)
    {
        var ids = await _schoolRepository.CreateSchoolAsync(new SchoolEntry
        {
            SchoolName = schoolName,
            SchoolCountry = schoolCountry
        });
        
        var facultyId = await _facultyRepository.CreateFacultyAsync(new FacultyEntry
        {
            SchoolId = ids[0],
            IdentityId = identityId,
            FirstName = firstName,
            LastName = lastName,
            Email = signinEmail,
        }, [ids[1]]);

        await _schoolRepository.CreateCycleAsync(new CycleEntry
        {
            SchoolId = ids[0],
            IsActive = true,
            CycleName = "Default Cycle",
            Code = "DEF",
            ScheduleType = 1,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
        });
    }
}