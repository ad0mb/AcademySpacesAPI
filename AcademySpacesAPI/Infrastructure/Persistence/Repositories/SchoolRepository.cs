using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.Context;
using AcademySpacesAPI.Entities;
using AcademySpacesAPI.Exceptions;

namespace AcademySpacesAPI.Infrastructure.Persistence.Repositories;

public class SchoolRepository : ISchoolRepository
{
    
    private readonly MyDbContext _context;
    private readonly IRoleRepository _roleRepository;
    
    public SchoolRepository(MyDbContext context, IRoleRepository roleRepository)
    {
        _context = context;
        _roleRepository = roleRepository;
    }
    
    //TODO: Exception Handling (use result), return exception meant for core
    //Db update exception
    public async Task<int[]> CreateSchoolAsync(CreateSchoolEntry request)
    {
        var school = new School
        {
            Name = request.SchoolName,
            CountryOfOrigin = request.SchoolCountry,
        };
        
        await _context.Schools.AddAsync(school);
        var result = await _context.SaveChangesAsync();
        if (result == 0)
        {
            throw new NoRowsAffectedException("No rows were affected when creating the school.");
        }

        //TODO: Add permissions to add with each role later
        var chiefAdminRoleId = await _roleRepository.CreateRoleAsync(
            new CreateRoleEntry  { SchoolId = school.SchoolId, RoleName = "ChiefAdministrator" }, ["ChiefAdministrator:000"]);
        await _roleRepository.CreateRoleAsync(new CreateRoleEntry { SchoolId = school.SchoolId, RoleName = "Administrator"}, ["Administrator:000"]);
        await _roleRepository.CreateRoleAsync(new CreateRoleEntry  { SchoolId = school.SchoolId, RoleName = "Teacher" }, []);
        await _roleRepository.CreateRoleAsync(new CreateRoleEntry  { SchoolId = school.SchoolId, RoleName = "Parent" }, []);
        await _roleRepository.CreateRoleAsync(new CreateRoleEntry  { SchoolId = school.SchoolId, RoleName = "Student" }, []);
        
        return [school.SchoolId, chiefAdminRoleId];
    }
}