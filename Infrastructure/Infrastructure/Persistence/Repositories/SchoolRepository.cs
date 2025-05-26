using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

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
    public async Task<int[]> CreateSchoolAsync(SchoolEntry request)
    {
        try
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
                new RoleEntry { SchoolId = school.SchoolId, RoleName = "ChiefAdministrator" },
                ["ChiefAdministrator:000"]);
            await _roleRepository.CreateRoleAsync(
                new RoleEntry { SchoolId = school.SchoolId, RoleName = "Administrator" }, ["Administrator:000"]);
            await _roleRepository.CreateRoleAsync(
                new RoleEntry { SchoolId = school.SchoolId, RoleName = "Teacher" }, []);
            await _roleRepository.CreateRoleAsync(
                new RoleEntry { SchoolId = school.SchoolId, RoleName = "Parent" }, []);
            await _roleRepository.CreateRoleAsync(
                new RoleEntry { SchoolId = school.SchoolId, RoleName = "Student" }, []);

            return [school.SchoolId, chiefAdminRoleId];
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding school to the database", ex);
        }
    }
}