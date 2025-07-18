using Core.ApplicationCore.DomainEntities;
using Core.ApplicationCore.Interfaces.Adapters;
using Core.Exceptions;
using Infrastructure.Infrastructure.Persistence.Context;
using Infrastructure.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Infrastructure.Persistence.Repositories;

public class FacultyRepository : IFacultyRepository
{

    private readonly MyDbContext _context;

    public FacultyRepository(MyDbContext context)
    {
        _context = context;
    }

    //TODO: Exception Handling (use result), return exception meant for core
    public async Task<int> CreateFacultyAsync(FacultyEntry request)
    {
        try
        {
            var existingEmail = await (from f in _context.Faculties
                where f.Email == request.Email && f.SchoolId == request.SchoolId
                select f.Email).FirstOrDefaultAsync();
            
            if (existingEmail == request.Email)
            {
                throw new DuplicateEmailException("Email already exists");
            }
            
            var faculty = new Faculty
            {
                SchoolId = request.SchoolId,
                IdentityId = request.IdentityId,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
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
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding faculty to the database", ex);
        }
    }

    public async Task<int> CreateFacultyAsync(FacultyEntry request, int[] roleIds)
    {
        try
        {
            var existingEmail = await (from f in _context.Faculties
                where f.Email == request.Email && f.SchoolId == request.SchoolId
                select f.Email).FirstOrDefaultAsync();
            
            if (existingEmail == request.Email)
            {
                throw new DuplicateEmailException("Email already exists");
            }
            
            var faculty = new Faculty
            {
                SchoolId = request.SchoolId,
                IdentityId = request.IdentityId,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
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
            
            foreach (var roleId in roleIds)
            {
                await AddRoleToFacultyAsync(new FacultyRoleEntry
                {
                    FacultyId = faculty.FacultyId,
                    RoleId = roleId
                });
            }
            
            return faculty.FacultyId;
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding faculty to the database", ex);
        }
    }

    public async Task AddRoleToFacultyAsync(FacultyRoleEntry request)
    {
        try
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
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue adding faculty role to the database", ex);
        }
    }

    public async Task<FacultyEntry?> GetFacultyByIdentityIdAsync(string identityId)
    {
        try
        {
            var faculty = await (from f in _context.Faculties
                where f.IdentityId == identityId
                select f).FirstOrDefaultAsync();

            if (faculty == null)
            {
                return null;
            }
            
            var facultyEntry = new FacultyEntry
            {
                FacultyId = faculty.FacultyId,
                SchoolId = faculty.SchoolId,
                IdentityId = faculty.IdentityId,
                FirstName = faculty.FirstName,
                MiddleName = faculty.MiddleName,
                LastName = faculty.LastName,
                PhoneNumber = faculty.PhoneNumber,
                Email = faculty.Email,
                DateCreated = faculty.DateCreated,
                DateUpdated = faculty.DateModified
            };

            return facultyEntry;
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue retrieving faculty from the database", ex);
        }
    }
    
    public async Task<(List<FacultyEntry> facultyList, int totalCount)> GetFacultyBySchoolIdAsync(int schoolId, int pageSize, int pageNumber, string? searchTerm)
    {
        try
        {
            List<FacultyEntry> faculty = new List<FacultyEntry>();

            IQueryable<Faculty> query = from f in _context.Faculties
                where f.SchoolId == schoolId

                      //Search filter
                      && (
                          string.IsNullOrEmpty(searchTerm) // If searchTerm is null or empty, return all classrooms
                          || (
                              f != null &&
                              ( // Search by teacher's name
                                  (f.FirstName != null && f.FirstName.ToLower().Contains(searchTerm)) ||
                                  (f.MiddleName != null && f.MiddleName.ToLower().Contains(searchTerm)) ||
                                  (f.LastName != null && f.LastName.ToLower().Contains(searchTerm))
                              )
                          )
                      )

                orderby f.FacultyId
                select f;
            
            var totalCount = await query.CountAsync();
            
            if (pageSize > 0 && pageNumber > 0)
            {
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
                
            var dbFaculty = await query.ToListAsync();
            

            foreach (var user in dbFaculty)
            {
                faculty.Add(new FacultyEntry
                {
                    FacultyId = user.FacultyId,
                    SchoolId = user.SchoolId,
                    IdentityId = user.IdentityId,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    DateCreated = user.DateCreated,
                    DateUpdated = user.DateModified
                });
            }
            
            return (faculty, totalCount);
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue retrieving faculty from the database", ex);
        }
    }

    public async Task UpdateFacultyAsync(FacultyEntry request)
    {
        try
        {
            var faculty =
                await (from f in _context.Faculties
                    where f.FacultyId == request.FacultyId
                    select f).FirstOrDefaultAsync();
            
            if (faculty == null)
            {
                throw new NotFoundException("Role to update not found.");
            }
            
            faculty.FirstName = request.FirstName;
            faculty.MiddleName = request.MiddleName;
            faculty.LastName = request.LastName;
            faculty.PhoneNumber = request.PhoneNumber;
            
            var result = await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new DbException("Issue updating faculty in the database", ex);
        }
    }
}