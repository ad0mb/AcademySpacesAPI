using AcademySpacesAPI.ApplicationCore.DomainEntities;
using AcademySpacesAPI.ApplicationCore.Interfaces.Adapters;
using AcademySpacesAPI.Exceptions;
using AcademySpacesAPI.Infrastructure.Persistence.Context;
using AcademySpacesAPI.Infrastructure.Persistence.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AcademySpacesAPI.Infrastructure.Persistence.Repositories;

public class FacultyRepository : IFacultyRepository
{
    
    private readonly MyDbContext _context;
    
    public FacultyRepository(MyDbContext context)
    {
        _context = context;
    }
    
    //TODO: Exception Handling (use result), return exception meant for core
    public async Task<int> CreateFacultyAsync(CreateFacultyEntry request)
    {
        var faculty = new Faculty
        {
            SchoolId = request.SchoolId,
            IdentityId = request.IdentityId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
        };
        
        await _context.Faculties.AddAsync(faculty);
        var result = await _context.SaveChangesAsync();
        if (result == 0)
        {
            throw new NoRowsAffectedException("Faculty not created");
        }
        return faculty.FacultyId;
    }

    public async Task<int> CreateFacultyAsync(CreateFacultyEntry request, int[] roleIds)
    {
        var faculty = new Faculty
        {
            SchoolId = request.SchoolId,
            IdentityId = request.IdentityId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
        };
        
        await _context.Faculties.AddAsync(faculty);
        var result = await _context.SaveChangesAsync();
        if (result == 0)
        {
            throw new NoRowsAffectedException("Faculty not created");
        }

        if (!roleIds.IsNullOrEmpty())
        {
            foreach (var roleId in roleIds)
            {
                await AddRoleToFacultyAsync(new CreateFacultyRoleEntry
                {
                    FacultyId = faculty.FacultyId,
                    RoleId = roleId
                });
            }
        }

        return faculty.FacultyId;
    }

    public async Task AddRoleToFacultyAsync(CreateFacultyRoleEntry request)
    {
        var facultyRole = new FacultyRole
        {
            FacultyId = request.FacultyId,
            RoleId = request.RoleId
        };
        
        await _context.FacultyRoles.AddAsync(facultyRole);
        var result = await _context.SaveChangesAsync();
        if (result == 0)
        {
            throw new NoRowsAffectedException("Faculty role not created");
        }
    }
}